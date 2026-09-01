using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Konu;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class KonularController : Controller
{
    private readonly IKonuServisi _konuServisi;
    private readonly IDersServisi _dersServisi;
    private readonly IOgrenciServisi _ogrenciServisi;

    public KonularController(IKonuServisi konuServisi, IDersServisi dersServisi, IOgrenciServisi ogrenciServisi)
    {
        _konuServisi = konuServisi;
        _dersServisi = dersServisi;
        _ogrenciServisi = ogrenciServisi;
    }

    public async Task<IActionResult> Index(int? dersId)
    {
        var model = new KonuListeViewModel
        {
            Konular = await _konuServisi.ListeleAsync(dersId),
            DersSecenekleri = await _dersServisi.ListeleAsync(),
            DersId = dersId
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle()
    {
        var model = new KonuFormViewModel
        {
            Konu = new KonuKaydetDTO { AktifMi = true },
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(KonuKaydetDTO konu)
    {
        var sonuc = await _konuServisi.EkleAsync(konu);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new KonuFormViewModel { Konu = konu, DersSecenekleri = await _dersServisi.ListeleAsync() });
        }

        TempData["BasariMesaji"] = "Konu eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var konu = await _konuServisi.DuzenlemeIcinGetirAsync(id);
        if (konu is null)
        {
            return NotFound();
        }

        var model = new KonuFormViewModel
        {
            Konu = konu,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(KonuKaydetDTO konu)
    {
        var sonuc = await _konuServisi.GuncelleAsync(konu);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new KonuFormViewModel { Konu = konu, DersSecenekleri = await _dersServisi.ListeleAsync() });
        }

        TempData["BasariMesaji"] = "Konu güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _konuServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Konu silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Takip(int? ogrenciId)
    {
        var model = new KonuTakipViewModel
        {
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            OgrenciId = ogrenciId
        };

        if (ogrenciId.HasValue)
        {
            model.Takipler = await _konuServisi.TumKonularIcinTakipDurumlariniGetirAsync(ogrenciId.Value);
            model.DersIlerlemeleri = await _konuServisi.DersBazliIlerlemeHesaplaAsync(ogrenciId.Value);
            model.ZayifKonuOnerileri = await _konuServisi.ZayifKonuOnerileriHesaplaAsync(ogrenciId.Value);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DurumIsaretle(int ogrenciId, int konuId, KonuTakipDurumu durum)
    {
        var sonuc = await _konuServisi.DurumIsaretleAsync(new OgrenciKonuTakipKaydetDTO
        {
            OgrenciId = ogrenciId,
            KonuId = konuId,
            Durum = durum
        });

        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Konu durumu güncellendi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Takip), new { ogrenciId });
    }
}
