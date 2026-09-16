using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class HedeflerController : Controller
{
    private readonly IHedefServisi _hedefServisi;
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IDersServisi _dersServisi;

    public HedeflerController(IHedefServisi hedefServisi, IOgrenciServisi ogrenciServisi, IDersServisi dersServisi)
    {
        _hedefServisi = hedefServisi;
        _ogrenciServisi = ogrenciServisi;
        _dersServisi = dersServisi;
    }

    public async Task<IActionResult> Index(int? ogrenciId)
    {
        var model = new HedefIndexViewModel
        {
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            OgrenciId = ogrenciId
        };

        if (ogrenciId.HasValue)
        {
            var tumHedefler = await _hedefServisi.ListeleAsync(ogrenciId.Value);
            model.AktifHedef = tumHedefler.FirstOrDefault(h => h.AktifMi);
            model.Gecmis = tumHedefler.Where(h => !h.AktifMi).ToList();
            model.Karsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(ogrenciId.Value);
        }

        return View(model);
    }

    public async Task<IActionResult> Ekle(int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var dersler = await _dersServisi.ListeleAsync();

        // öğrencinin bulunduğu alana göre en uygun puan türünü baştan seçili getiriyoruz, formu hiç değiştirmeden yanlış puan türüyle kaydedilmesin diye
        var onerilenPuanTuru = ogrenci.Alan switch
        {
            Alan.Sayisal => PuanTuru.Say,
            Alan.EsitAgirlik => PuanTuru.Ea,
            Alan.Sozel => PuanTuru.Soz,
            Alan.Dil => PuanTuru.Dil,
            _ => PuanTuru.Say
        };

        var model = new HedefFormViewModel
        {
            Hedef = new HedefKaydetDTO { OgrenciId = ogrenciId, PuanTuru = onerilenPuanTuru },
            OgrenciAdSoyad = ogrenci.AdSoyad,
            DersNetSatirlari = dersler.Select(d => new HedefDersNetiSatiriViewModel { DersId = d.Id, DersAdi = d.Ad }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(HedefKaydetDTO hedef, List<HedefDersNetiSatiriViewModel> dersNetSatirlari)
    {
        hedef.DersNetleri = DersNetleriniCikar(dersNetSatirlari);

        var sonuc = await _hedefServisi.EkleAsync(hedef);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(hedef.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new HedefFormViewModel
            {
                Hedef = hedef,
                OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
                DersNetSatirlari = dersNetSatirlari
            });
        }

        TempData["BasariMesaji"] = "Hedef kaydedildi.";
        return RedirectToAction(nameof(Index), new { ogrenciId = hedef.OgrenciId });
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var hedef = await _hedefServisi.DuzenlemeIcinGetirAsync(id);
        if (hedef is null)
        {
            return NotFound();
        }

        var ogrenci = await _ogrenciServisi.TekGetirAsync(hedef.OgrenciId);
        var dersler = await _dersServisi.ListeleAsync();

        var model = new HedefFormViewModel
        {
            Hedef = hedef,
            OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
            DersNetSatirlari = dersler.Select(d =>
            {
                var mevcut = hedef.DersNetleri.FirstOrDefault(dn => dn.DersId == d.Id);
                return new HedefDersNetiSatiriViewModel
                {
                    Id = mevcut?.Id ?? 0,
                    DersId = d.Id,
                    DersAdi = d.Ad,
                    HedefNet = mevcut?.HedefNet
                };
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(HedefKaydetDTO hedef, List<HedefDersNetiSatiriViewModel> dersNetSatirlari)
    {
        hedef.DersNetleri = DersNetleriniCikar(dersNetSatirlari);

        var sonuc = await _hedefServisi.GuncelleAsync(hedef);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(hedef.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new HedefFormViewModel
            {
                Hedef = hedef,
                OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
                DersNetSatirlari = dersNetSatirlari
            });
        }

        TempData["BasariMesaji"] = "Hedef güncellendi.";
        return RedirectToAction(nameof(Index), new { ogrenciId = hedef.OgrenciId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id, int ogrenciId)
    {
        var sonuc = await _hedefServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Hedef silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index), new { ogrenciId });
    }

    // formdan gelen satırlardan sadece net girilmiş olanları hedef kaydına dahil ediyoruz
    private static List<HedefDersNetiKaydetDTO> DersNetleriniCikar(List<HedefDersNetiSatiriViewModel> satirlar)
    {
        return satirlar
            .Where(s => s.HedefNet.HasValue)
            .Select(s => new HedefDersNetiKaydetDTO { Id = s.Id, DersId = s.DersId, HedefNet = s.HedefNet!.Value })
            .ToList();
    }
}
