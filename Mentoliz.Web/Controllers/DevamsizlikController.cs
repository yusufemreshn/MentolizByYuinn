using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class DevamsizlikController : Controller
{
    private readonly IDevamsizlikServisi _devamsizlikServisi;
    private readonly ISinifServisi _sinifServisi;

    public DevamsizlikController(IDevamsizlikServisi devamsizlikServisi, ISinifServisi sinifServisi)
    {
        _devamsizlikServisi = devamsizlikServisi;
        _sinifServisi = sinifServisi;
    }

    public async Task<IActionResult> Index(int? sinifId, DateTime? tarih)
    {
        var secilenTarih = (tarih ?? DateTime.Today).Date;

        var model = new DevamsizlikIndexViewModel
        {
            SinifSecenekleri = await _sinifServisi.ListeleAsync(),
            SinifId = sinifId,
            Tarih = secilenTarih
        };

        if (sinifId.HasValue)
        {
            model.Gun = await _devamsizlikServisi.SinifGunlukDurumGetirAsync(sinifId.Value, secilenTarih);
        }

        return View(model);
    }

    // sınıf seçili ekrandaki tek bir öğrenci satırı değiştiğinde çağrılıyor, sinifId sadece javascript kapalıyken geri dönülecek adresi kurmak için
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DurumKaydet(DevamsizlikKaydetDTO dto, int sinifId)
    {
        var sonuc = await _devamsizlikServisi.DurumKaydetAsync(dto);

        if (string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
        {
            return Json(new { basarili = sonuc.Basarili, mesaj = sonuc.Basarili ? null : string.Join(" ", sonuc.HataMesajlari) });
        }

        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Devamsızlık durumu kaydedildi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index), new { sinifId, tarih = dto.Tarih.ToString("yyyy-MM-dd") });
    }
}
