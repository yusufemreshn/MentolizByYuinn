using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class OdevlerController : Controller
{
    private readonly IOdevServisi _odevServisi;
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly ISinifServisi _sinifServisi;
    private readonly IDersServisi _dersServisi;

    public OdevlerController(
        IOdevServisi odevServisi,
        IOgrenciServisi ogrenciServisi,
        ISinifServisi sinifServisi,
        IDersServisi dersServisi)
    {
        _odevServisi = odevServisi;
        _ogrenciServisi = ogrenciServisi;
        _sinifServisi = sinifServisi;
        _dersServisi = dersServisi;
    }

    public async Task<IActionResult> Index(string? arama, int? sinifId, int? dersId, OdevDurumu? durum)
    {
        var filtre = new OdevFiltreDTO { Arama = arama, SinifId = sinifId, DersId = dersId, Durum = durum };

        var model = new OdevListeViewModel
        {
            Odevler = await _odevServisi.ListeleAsync(filtre),
            TopluAtamalar = await _odevServisi.TopluAtamalariListeleAsync(),
            SinifSecenekleri = await _sinifServisi.ListeleAsync(),
            DersSecenekleri = await _dersServisi.ListeleAsync(),
            Arama = arama,
            SinifId = sinifId,
            DersId = dersId,
            Durum = durum
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle(int? ogrenciId)
    {
        var model = new OdevFormViewModel
        {
            // öğrenci profilinden gelindiyse öğrenci baştan seçili gelsin diye
            Odev = new OdevKaydetDTO { OgrenciId = ogrenciId ?? 0, VerilisTarihi = DateTime.Today, SonTeslimTarihi = DateTime.Today, Durum = OdevDurumu.Verildi },
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(OdevKaydetDTO odev)
    {
        var sonuc = await _odevServisi.EkleAsync(odev);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new OdevFormViewModel
            {
                Odev = odev,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Ödev eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var odev = await _odevServisi.DuzenlemeIcinGetirAsync(id);
        if (odev is null)
        {
            return NotFound();
        }

        var model = new OdevFormViewModel
        {
            Odev = odev,
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(OdevKaydetDTO odev)
    {
        var sonuc = await _odevServisi.GuncelleAsync(odev);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new OdevFormViewModel
            {
                Odev = odev,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Ödev güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DurumGuncelle(int id, OdevDurumu yeniDurum, string? donusUrl)
    {
        var sonuc = await _odevServisi.DurumGuncelleAsync(id, yeniDurum);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Ödev durumu güncellendi."
            : string.Join(" ", sonuc.HataMesajlari);

        // öğrenci detayındaki ödev sekmesinden çağrıldıysa oraya geri dönüyoruz, yoksa varsayılan olarak ödev listesine
        if (!string.IsNullOrEmpty(donusUrl) && Url.IsLocalUrl(donusUrl))
        {
            return Redirect(donusUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _odevServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Ödev silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    // bir toplu ödev atamasındaki bütün öğrencilerin durumunu tek ekranda görüp değiştirmek için
    public async Task<IActionResult> Yoklama(int? topluAtamaId)
    {
        var model = new OdevYoklamaViewModel
        {
            TopluAtamalar = await _odevServisi.TopluAtamalariListeleAsync(),
            SeciliTopluAtamaId = topluAtamaId,
            Odevler = topluAtamaId.HasValue
                ? await _odevServisi.ListeleAsync(new OdevFiltreDTO { TopluAtamaId = topluAtamaId.Value })
                : []
        };

        return View(model);
    }

    public async Task<IActionResult> TopluAtamaEkle()
    {
        var model = new TopluAtamaFormViewModel
        {
            TopluAtama = new OdevTopluAtamaKaydetDTO { VerilisTarihi = DateTime.Today, SonTeslimTarihi = DateTime.Today },
            SinifSecenekleri = await _sinifServisi.ListeleAsync(),
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TopluAtamaEkle(OdevTopluAtamaKaydetDTO topluAtama)
    {
        var sonuc = await _odevServisi.TopluAtamaEkleAsync(topluAtama);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new TopluAtamaFormViewModel
            {
                TopluAtama = topluAtama,
                SinifSecenekleri = await _sinifServisi.ListeleAsync(),
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = $"Toplu ödev {sonuc.Veri!.OdevSayisi} öğrenciye verildi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TopluAtamaSil(int id)
    {
        var sonuc = await _odevServisi.TopluAtamaSilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Toplu atama silindi, öğrencilerin bireysel ödev kayıtları korundu."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }
}
