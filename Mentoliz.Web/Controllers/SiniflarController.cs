using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Business.Dto.Sinif;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class SiniflarController : Controller
{
    private readonly ISinifServisi _sinifServisi;
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IDersServisi _dersServisi;
    private readonly IDersProgramiServisi _dersProgramiServisi;

    public SiniflarController(
        ISinifServisi sinifServisi,
        IOgrenciServisi ogrenciServisi,
        IDersServisi dersServisi,
        IDersProgramiServisi dersProgramiServisi)
    {
        _sinifServisi = sinifServisi;
        _ogrenciServisi = ogrenciServisi;
        _dersServisi = dersServisi;
        _dersProgramiServisi = dersProgramiServisi;
    }

    public async Task<IActionResult> Index()
    {
        var model = new SinifListeViewModel
        {
            Siniflar = await _sinifServisi.ListeleAsync()
        };

        return View(model);
    }

    public IActionResult Ekle()
    {
        var model = new SinifFormViewModel
        {
            Sinif = new SinifKaydetDTO { AktifMi = true }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(SinifKaydetDTO sinif)
    {
        var sonuc = await _sinifServisi.EkleAsync(sinif);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new SinifFormViewModel { Sinif = sinif });
        }

        TempData["BasariMesaji"] = "Sınıf başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var sinif = await _sinifServisi.DuzenlemeIcinGetirAsync(id);
        if (sinif is null)
        {
            return NotFound();
        }

        return View(new SinifFormViewModel { Sinif = sinif });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(SinifKaydetDTO sinif)
    {
        var sonuc = await _sinifServisi.GuncelleAsync(sinif);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new SinifFormViewModel { Sinif = sinif });
        }

        TempData["BasariMesaji"] = "Sınıf başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _sinifServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Sınıf silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detay(int id)
    {
        var sinif = await _sinifServisi.TekGetirAsync(id);
        if (sinif is null)
        {
            return NotFound();
        }

        var model = new SinifDetayViewModel
        {
            Sinif = sinif,
            SinifOgrencileri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { SinifId = id }),
            // öğrenci atama açılır kutusu bütün aktif öğrencileri listeliyor, sadece bu sınıftakileri değil
            AktifOgrenciler = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            DersProgrami = await _dersProgramiServisi.ListeleAsync(id)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciAta(int sinifId, int ogrenciId)
    {
        // ayrı bir atama servisi yok, öğrencinin sınıf alanını güncelleyip mevcut kaydet akışını kullanıyoruz
        var ogrenci = await _ogrenciServisi.DuzenlemeIcinGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        ogrenci.SinifId = sinifId;
        var sonuc = await _ogrenciServisi.GuncelleAsync(ogrenci);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Öğrenci sınıfa atandı."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = sinifId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciCikar(int sinifId, int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.DuzenlemeIcinGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        ogrenci.SinifId = null;
        var sonuc = await _ogrenciServisi.GuncelleAsync(ogrenci);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Öğrenci sınıftan çıkarıldı."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = sinifId });
    }

    public async Task<IActionResult> DersProgramiEkle(int sinifId)
    {
        var sinif = await _sinifServisi.TekGetirAsync(sinifId);
        if (sinif is null)
        {
            return NotFound();
        }

        var model = new DersProgramiFormViewModel
        {
            Satir = new DersProgramiSatiriKaydetDTO { SinifId = sinifId },
            SinifAdi = sinif.Ad,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DersProgramiEkle(DersProgramiSatiriKaydetDTO satir)
    {
        var sonuc = await _dersProgramiServisi.EkleAsync(satir);
        if (!sonuc.Basarili)
        {
            var sinif = await _sinifServisi.TekGetirAsync(satir.SinifId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new DersProgramiFormViewModel
            {
                Satir = satir,
                SinifAdi = sinif?.Ad ?? string.Empty,
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Ders programına saat eklendi.";
        return RedirectToAction(nameof(Detay), new { id = satir.SinifId });
    }

    public async Task<IActionResult> DersProgramiDuzenle(int id)
    {
        var satir = await _dersProgramiServisi.DuzenlemeIcinGetirAsync(id);
        if (satir is null)
        {
            return NotFound();
        }

        var sinif = await _sinifServisi.TekGetirAsync(satir.SinifId);

        var model = new DersProgramiFormViewModel
        {
            Satir = satir,
            SinifAdi = sinif?.Ad ?? string.Empty,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DersProgramiDuzenle(DersProgramiSatiriKaydetDTO satir)
    {
        var sonuc = await _dersProgramiServisi.GuncelleAsync(satir);
        if (!sonuc.Basarili)
        {
            var sinif = await _sinifServisi.TekGetirAsync(satir.SinifId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new DersProgramiFormViewModel
            {
                Satir = satir,
                SinifAdi = sinif?.Ad ?? string.Empty,
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Ders programı satırı güncellendi.";
        return RedirectToAction(nameof(Detay), new { id = satir.SinifId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DersProgramiSil(int id, int sinifId)
    {
        var sonuc = await _dersProgramiServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Ders programı satırı silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = sinifId });
    }
}
