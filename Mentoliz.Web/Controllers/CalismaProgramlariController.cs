using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class CalismaProgramlariController : Controller
{
    private readonly ICalismaProgramiServisi _calismaProgramiServisi;
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IDersServisi _dersServisi;

    public CalismaProgramlariController(ICalismaProgramiServisi calismaProgramiServisi, IOgrenciServisi ogrenciServisi, IDersServisi dersServisi)
    {
        _calismaProgramiServisi = calismaProgramiServisi;
        _ogrenciServisi = ogrenciServisi;
        _dersServisi = dersServisi;
    }

    public async Task<IActionResult> Index(int? ogrenciId)
    {
        var model = new CalismaProgramiListeViewModel
        {
            OgrenciSecenekleri = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true }),
            OgrenciId = ogrenciId,
            Programlar = ogrenciId.HasValue ? await _calismaProgramiServisi.ListeleAsync(ogrenciId.Value) : []
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle(int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var model = new CalismaProgramiFormViewModel
        {
            Program = new CalismaProgramiKaydetDTO { OgrenciId = ogrenciId, HaftaBaslangicTarihi = DateTime.Today },
            OgrenciAdSoyad = ogrenci.AdSoyad
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(CalismaProgramiKaydetDTO program)
    {
        var sonuc = await _calismaProgramiServisi.EkleAsync(program);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(program.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new CalismaProgramiFormViewModel { Program = program, OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty });
        }

        TempData["BasariMesaji"] = "Çalışma programı oluşturuldu.";
        return RedirectToAction(nameof(Detay), new { id = sonuc.Veri!.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OncekiHaftadanKopyala(int ogrenciId, DateTime haftaBaslangicTarihi)
    {
        var sonuc = await _calismaProgramiServisi.OncekiHaftadanKopyalaAsync(ogrenciId, haftaBaslangicTarihi);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return RedirectToAction(nameof(Index), new { ogrenciId });
        }

        TempData["BasariMesaji"] = "Önceki haftanın programı kopyalandı.";
        return RedirectToAction(nameof(Detay), new { id = sonuc.Veri!.Id });
    }

    public async Task<IActionResult> Detay(int id)
    {
        var program = await _calismaProgramiServisi.TekGetirAsync(id);
        if (program is null)
        {
            return NotFound();
        }

        var ogrenci = await _ogrenciServisi.TekGetirAsync(program.OgrenciId);

        var model = new CalismaProgramiDetayViewModel
        {
            Program = program,
            OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id, int ogrenciId)
    {
        var sonuc = await _calismaProgramiServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Çalışma programı silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index), new { ogrenciId });
    }

    public async Task<IActionResult> SatirEkle(int calismaProgramiId)
    {
        var program = await _calismaProgramiServisi.TekGetirAsync(calismaProgramiId);
        if (program is null)
        {
            return NotFound();
        }

        var model = new CalismaSatirFormViewModel
        {
            CalismaProgramiId = calismaProgramiId,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SatirEkle(int calismaProgramiId, CalismaProgramiSatiriKaydetDTO satir)
    {
        var sonuc = await _calismaProgramiServisi.SatirEkleAsync(calismaProgramiId, satir);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new CalismaSatirFormViewModel
            {
                CalismaProgramiId = calismaProgramiId,
                Satir = satir,
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Çalışma satırı eklendi.";
        return RedirectToAction(nameof(Detay), new { id = calismaProgramiId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SatirSil(int id, int calismaProgramiId)
    {
        var sonuc = await _calismaProgramiServisi.SatirSilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Çalışma satırı silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = calismaProgramiId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SatirTamamlandi(int id, bool tamamlandiMi, int calismaProgramiId)
    {
        var sonuc = await _calismaProgramiServisi.SatirTamamlandiIsaretleAsync(id, tamamlandiMi);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Satır güncellendi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = calismaProgramiId });
    }
}
