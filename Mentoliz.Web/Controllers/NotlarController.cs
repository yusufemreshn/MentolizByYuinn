using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Not;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class NotlarController : Controller
{
    private readonly INotServisi _notServisi;

    public NotlarController(INotServisi notServisi)
    {
        _notServisi = notServisi;
    }

    public async Task<IActionResult> Index()
    {
        var model = new NotListeViewModel
        {
            Notlar = await _notServisi.ListeleAsync()
        };

        return View(model);
    }

    public IActionResult Ekle()
    {
        return View(new NotFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(NotKaydetDTO not)
    {
        var sonuc = await _notServisi.EkleAsync(not);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new NotFormViewModel { Not = not });
        }

        TempData["BasariMesaji"] = "Not eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var not = await _notServisi.DuzenlemeIcinGetirAsync(id);
        if (not is null)
        {
            return NotFound();
        }

        return View(new NotFormViewModel { Not = not });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(NotKaydetDTO not)
    {
        var sonuc = await _notServisi.GuncelleAsync(not);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new NotFormViewModel { Not = not });
        }

        TempData["BasariMesaji"] = "Not güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _notServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Not silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }
}
