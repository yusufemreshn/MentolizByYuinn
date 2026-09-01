using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Web.Helpers;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class DenemelerController : Controller
{
    private readonly IDenemeServisi _denemeServisi;
    private readonly ISinavTuruServisi _sinavTuruServisi;
    private readonly ISinifServisi _sinifServisi;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;

    public DenemelerController(
        IDenemeServisi denemeServisi,
        ISinavTuruServisi sinavTuruServisi,
        ISinifServisi sinifServisi,
        IDenemeAnaliziServisi denemeAnaliziServisi)
    {
        _denemeServisi = denemeServisi;
        _sinavTuruServisi = sinavTuruServisi;
        _sinifServisi = sinifServisi;
        _denemeAnaliziServisi = denemeAnaliziServisi;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DenemeListeViewModel
        {
            Denemeler = await _denemeServisi.ListeleAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle()
    {
        var model = new DenemeFormViewModel
        {
            Deneme = new DenemeKaydetDTO { Tarih = DateTime.Today },
            SinavTuruSecenekleri = await _sinavTuruServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(DenemeKaydetDTO deneme)
    {
        var sonuc = await _denemeServisi.EkleAsync(deneme);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new DenemeFormViewModel
            {
                Deneme = deneme,
                SinavTuruSecenekleri = await _sinavTuruServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Deneme oluşturuldu.";
        return RedirectToAction(nameof(Detay), new { id = sonuc.Veri!.Id });
    }

    // alan bazlı sekiz grafiğin gösterildiği detaylı analiz sayfası, ana sayfadaki sınıf ortalama net grafiğinden linkleniyor
    public async Task<IActionResult> Analiz()
    {
        var model = new DenemeAnaliziViewModel
        {
            AlanBazliSonuclar = await _denemeAnaliziServisi.AlanBazliOrtalamaNetleriHesaplaAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var deneme = await _denemeServisi.DuzenlemeIcinGetirAsync(id);
        if (deneme is null)
        {
            return NotFound();
        }

        var model = new DenemeFormViewModel
        {
            Deneme = deneme,
            SinavTuruSecenekleri = await _sinavTuruServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(DenemeKaydetDTO deneme)
    {
        var sonuc = await _denemeServisi.GuncelleAsync(deneme);
        if (!sonuc.Basarili)
        {
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new DenemeFormViewModel
            {
                Deneme = deneme,
                SinavTuruSecenekleri = await _sinavTuruServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Deneme güncellendi.";
        return RedirectToAction(nameof(Detay), new { id = deneme.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _denemeServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Deneme silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detay(int id)
    {
        var deneme = await _denemeServisi.TekGetirAsync(id);
        if (deneme is null)
        {
            return NotFound();
        }

        var sinavTuru = await _sinavTuruServisi.TekGetirAsync(deneme.SinavTuruId);

        var model = new DenemeDetayViewModel
        {
            Deneme = deneme,
            SinavTuru = sinavTuru ?? new SinavTuruDTO(),
            Sonuclar = await _denemeServisi.SonuclariListeleAsync(id),
            SinifSecenekleri = await _sinifServisi.ListeleAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> SonuclariCsvDisaAktar(int id)
    {
        var deneme = await _denemeServisi.TekGetirAsync(id);
        if (deneme is null)
        {
            return NotFound();
        }

        var sinavTuru = await _sinavTuruServisi.TekGetirAsync(deneme.SinavTuruId);
        var testler = (sinavTuru?.Testler ?? []).OrderBy(t => t.Sira).ToList();
        var sonuclar = await _denemeServisi.SonuclariListeleAsync(id);

        var basliklar = new List<string> { "Öğrenci" };
        basliklar.AddRange(testler.Select(t => t.DersAdi));
        basliklar.Add("Toplam Net");
        basliklar.Add("Durum");

        var satirlar = sonuclar.OrderBy(s => s.OgrenciAdSoyad).Select(sonuc =>
        {
            var alanlar = new List<string> { sonuc.OgrenciAdSoyad };
            alanlar.AddRange(testler.Select(test =>
            {
                var detay = sonuc.Detaylar.FirstOrDefault(d => d.SinavTuruTestId == test.Id);
                return detay is null ? "-" : detay.Net.ToString("0.##");
            }));
            alanlar.Add(sonuc.ToplamNet.ToString("0.##"));
            alanlar.Add(sonuc.KatilmadiMi ? "Katılmadı" : "Katıldı");
            return alanlar;
        });

        return CsvYazici.OlusturDosya(basliklar, satirlar, $"deneme-{id}-sonuclar.csv");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SonucSil(int id, int denemeId)
    {
        var sonuc = await _denemeServisi.SonucSilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Deneme sonucu silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = denemeId });
    }

    public async Task<IActionResult> SonucGirisi(int denemeId, int? sinifId)
    {
        var deneme = await _denemeServisi.TekGetirAsync(denemeId);
        if (deneme is null)
        {
            return NotFound();
        }

        var sinavTuru = await _sinavTuruServisi.TekGetirAsync(deneme.SinavTuruId);

        var model = new SonucGirisiViewModel
        {
            Deneme = deneme,
            SinavTuru = sinavTuru ?? new SinavTuruDTO(),
            SinifSecenekleri = await _sinifServisi.ListeleAsync(),
            SinifId = sinifId,
            Satirlar = sinifId.HasValue
                ? await _denemeServisi.SonucGirisiSatirlariniHazirlaAsync(denemeId, sinifId.Value)
                : []
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SonucGirisi(int denemeId, int sinifId, List<DenemeSonucSatiriDTO> satirlar)
    {
        var hatalar = new List<string>();

        foreach (var satir in satirlar)
        {
            var sonucDto = satir.Sonuc;

            // hiç dokunulmamış, daha önce kaydedilmemiş satırları boşuna kaydetmiyoruz
            var veriGirilmis = sonucDto.Id != 0 || sonucDto.KatilmadiMi || sonucDto.Detaylar.Any(d => d.Dogru != 0 || d.Yanlis != 0 || d.Bos != 0);
            if (!veriGirilmis)
            {
                continue;
            }

            sonucDto.DenemeId = denemeId;
            var sonuc = await _denemeServisi.SonucKaydetAsync(sonucDto);
            if (!sonuc.Basarili)
            {
                hatalar.Add($"{satir.OgrenciAdSoyad}: {string.Join(" ", sonuc.HataMesajlari)}");
            }
        }

        if (hatalar.Count > 0)
        {
            TempData["HataMesaji"] = string.Join(" ", hatalar);
        }
        else
        {
            TempData["BasariMesaji"] = "Deneme sonuçları kaydedildi.";
        }

        return RedirectToAction(nameof(SonucGirisi), new { denemeId, sinifId });
    }

    // önceki denemeden doldur düğmesi için, sınıftaki öğrencilerin bir önceki karşılaştırılabilir denemedeki sonuçlarını json olarak döner
    [HttpGet]
    public async Task<IActionResult> OncekiSonuclar(int denemeId, int sinifId)
    {
        var sonuc = await _denemeServisi.OncekiDenemeSonuclariniGetirAsync(denemeId, sinifId);
        return Json(sonuc);
    }

    // sonuç girişi ızgarasında bir satırdan çıkılınca sayfa yenilenmeden tek satırı kaydetmek için, gövde json geldiğinden token'ı başlıktan doğruluyoruz
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SatirKaydet([FromBody] DenemeSonucKaydetDTO sonuc)
    {
        var veriGirilmis = sonuc.Id != 0 || sonuc.KatilmadiMi || sonuc.Detaylar.Any(d => d.Dogru != 0 || d.Yanlis != 0 || d.Bos != 0);
        if (!veriGirilmis)
        {
            return Json(new { basarili = true, atlandi = true });
        }

        var kayitSonucu = await _denemeServisi.SonucKaydetAsync(sonuc);
        if (!kayitSonucu.Basarili)
        {
            return Json(new { basarili = false, hata = string.Join(" ", kayitSonucu.HataMesajlari) });
        }

        return Json(new
        {
            basarili = true,
            id = kayitSonucu.Veri!.Id,
            detaylar = kayitSonucu.Veri.Detaylar.Select(d => new { d.SinavTuruTestId, d.Id })
        });
    }
}
