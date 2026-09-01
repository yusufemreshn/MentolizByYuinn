using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

// görüşme, ödev teslimi, deneme ve görev kayıtlarını tek ay görünümünde birleştiren takvim
public class TakvimController : Controller
{
    private readonly ITakvimServisi _takvimServisi;

    public TakvimController(ITakvimServisi takvimServisi)
    {
        _takvimServisi = takvimServisi;
    }

    public async Task<IActionResult> Index(int? yil, int? ay)
    {
        var bugun = DateTime.Today;

        // ay değeri aralık dışına taşarsa (önceki/sonraki ay linklerinden) adddmonths kendisi yılı da düzeltiyor
        var ayBaslangici = new DateTime(yil ?? bugun.Year, 1, 1).AddMonths((ay ?? bugun.Month) - 1);
        var ayBitisi = ayBaslangici.AddMonths(1);

        var model = new TakvimIndexViewModel
        {
            AyBaslangici = ayBaslangici,
            Ogeler = await _takvimServisi.OgeleriGetirAsync(ayBaslangici, ayBitisi)
        };

        return View(model);
    }
}
