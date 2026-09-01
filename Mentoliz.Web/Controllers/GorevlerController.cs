using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class GorevlerController : Controller
{
    private readonly IGorevServisi _gorevServisi;
    private readonly IOgrenciServisi _ogrenciServisi;

    public GorevlerController(IGorevServisi gorevServisi, IOgrenciServisi ogrenciServisi)
    {
        _gorevServisi = gorevServisi;
        _ogrenciServisi = ogrenciServisi;
    }

    public async Task<IActionResult> Index(int? ogrenciId, bool? tamamlandiMi)
    {
        var filtre = new GorevFiltreDTO { OgrenciId = ogrenciId, TamamlandiMi = tamamlandiMi };

        var model = new GorevListeViewModel
        {
            Gorevler = await _gorevServisi.ListeleAsync(filtre),
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            OgrenciId = ogrenciId,
            TamamlandiMi = tamamlandiMi
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle(int? ogrenciId)
    {
        var model = new GorevFormViewModel
        {
            Gorev = new GorevKaydetDTO { Tarih = DateTime.Today, OgrenciId = ogrenciId },
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(GorevKaydetDTO gorev)
    {
        var sonuc = await _gorevServisi.EkleAsync(gorev);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new GorevFormViewModel
            {
                Gorev = gorev,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
            });
        }

        TempData["BasariMesaji"] = "Görev eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var gorev = await _gorevServisi.DuzenlemeIcinGetirAsync(id);
        if (gorev is null)
        {
            return NotFound();
        }

        var model = new GorevFormViewModel
        {
            Gorev = gorev,
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(GorevKaydetDTO gorev)
    {
        var sonuc = await _gorevServisi.GuncelleAsync(gorev);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new GorevFormViewModel
            {
                Gorev = gorev,
                OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true })
            });
        }

        TempData["BasariMesaji"] = "Görev güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Tamamlandi(int id, string? donusUrl)
    {
        var sonuc = await _gorevServisi.TamamlandiIsaretleAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Görev tamamlandı olarak işaretlendi."
            : string.Join(" ", sonuc.HataMesajlari);

        // bugün ekranından çağrıldıysa oraya geri dönüyoruz, yoksa varsayılan olarak görev listesine
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
        var sonuc = await _gorevServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Görev silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }
}
