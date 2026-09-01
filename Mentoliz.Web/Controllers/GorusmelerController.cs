using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class GorusmelerController : Controller
{
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IGorusmeHazirlikServisi _gorusmeHazirlikServisi;

    public GorusmelerController(IGorusmeServisi gorusmeServisi, IOgrenciServisi ogrenciServisi, IGorusmeHazirlikServisi gorusmeHazirlikServisi)
    {
        _gorusmeServisi = gorusmeServisi;
        _ogrenciServisi = ogrenciServisi;
        _gorusmeHazirlikServisi = gorusmeHazirlikServisi;
    }

    // görüşme formunda öğrenci seçilince yan panelde gösterilecek hazırlık özetini json olarak döner
    [HttpGet]
    public async Task<IActionResult> HazirlikOzeti(int ogrenciId)
    {
        var ozet = await _gorusmeHazirlikServisi.OzetGetirAsync(ogrenciId);
        return Json(ozet);
    }

    public async Task<IActionResult> Index(int? ogrenciId, GorusmeTuru? tur)
    {
        var filtre = new GorusmeFiltreDTO { OgrenciId = ogrenciId, Tur = tur };

        var model = new GorusmeListeViewModel
        {
            Gorusmeler = await _gorusmeServisi.ListeleAsync(filtre),
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            OgrenciId = ogrenciId,
            Tur = tur
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle(int? ogrenciId)
    {
        var model = new GorusmeFormViewModel
        {
            Gorusme = new GorusmeKaydetDTO { Tarih = DateTime.Today, OgrenciId = ogrenciId ?? 0 },
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(GorusmeKaydetDTO gorusme)
    {
        var sonuc = await _gorusmeServisi.EkleAsync(gorusme);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new GorusmeFormViewModel
            {
                Gorusme = gorusme,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
            });
        }

        TempData["BasariMesaji"] = gorusme.SonrakiGorusmeTarihi.HasValue
            ? "Görüşme kaydedildi, sonraki görüşme için görev listesine hatırlatma eklendi."
            : "Görüşme kaydedildi.";
        return RedirectToAction(nameof(Index), new { ogrenciId = gorusme.OgrenciId });
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var gorusme = await _gorusmeServisi.DuzenlemeIcinGetirAsync(id);
        if (gorusme is null)
        {
            return NotFound();
        }

        var model = new GorusmeFormViewModel
        {
            Gorusme = gorusme,
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(GorusmeKaydetDTO gorusme)
    {
        var sonuc = await _gorusmeServisi.GuncelleAsync(gorusme);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new GorusmeFormViewModel
            {
                Gorusme = gorusme,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
            });
        }

        TempData["BasariMesaji"] = "Görüşme güncellendi.";
        return RedirectToAction(nameof(Index), new { ogrenciId = gorusme.OgrenciId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _gorusmeServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Görüşme silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }
}
