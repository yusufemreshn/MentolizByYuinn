using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.Controllers;

// komut paletinin öğrenci arama sonuçlarını beslediği tek uçlu, sade bir denetleyici
public class AramaController : Controller
{
    // aramada en fazla bu kadar öğrenci gösteriliyor, liste uzayıp paleti taşırmasın diye
    private const int SonucAdedi = 8;

    private readonly IOgrenciServisi _ogrenciServisi;

    public AramaController(IOgrenciServisi ogrenciServisi)
    {
        _ogrenciServisi = ogrenciServisi;
    }

    [HttpGet]
    public async Task<IActionResult> Ogrenciler(string? q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
        {
            return Json(Array.Empty<object>());
        }

        var ogrenciler = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { Arama = q });

        var sonuc = ogrenciler.Take(SonucAdedi).Select(o => new
        {
            baslik = o.AdSoyad,
            alt = o.SinifAdi ?? "Sınıfsız",
            url = Url.Action("Detay", "Ogrenciler", new { id = o.Id })
        });

        return Json(sonuc);
    }
}
