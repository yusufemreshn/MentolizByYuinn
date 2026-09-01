using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Web.Helpers;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class AyarlarController : Controller
{
    private readonly IAyarServisi _ayarServisi;
    private readonly IYedeklemeServisi _yedeklemeServisi;
    private readonly IDemoVeriServisi _demoVeriServisi;

    public AyarlarController(IAyarServisi ayarServisi, IYedeklemeServisi yedeklemeServisi, IDemoVeriServisi demoVeriServisi)
    {
        _ayarServisi = ayarServisi;
        _yedeklemeServisi = yedeklemeServisi;
        _demoVeriServisi = demoVeriServisi;
    }

    public async Task<IActionResult> Index()
    {
        var model = await ModelOlusturAsync();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Kaydet(AyarlarViewModel model)
    {
        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.KurumAdi, model.KurumAdi ?? "");
        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.OgretmenAdi, model.OgretmenAdi ?? "");
        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.YedekKlasoru, model.YedekKlasoru ?? "");
        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.KapanistaOtomatikYedek, model.KapanistaOtomatikYedek.ToString());

        TempData["BasariMesaji"] = "Ayarlar kaydedildi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TemaSec(string tema)
    {
        // listede olmayan bir değer gelirse görmezden geliyoruz, data-theme özelliğine keyfi metin basılmasın
        if (!TemaSecenekleri.GecerliMi(tema))
        {
            TempData["HataMesaji"] = "Geçersiz tema seçimi.";
            return RedirectToAction(nameof(Index));
        }

        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.SeciliTema, tema);

        TempData["BasariMesaji"] = "Tema güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // gece modu üst çubuktaki simgeden her sayfada değiştirilebiliyor, bu yüzden ayarlar sayfasına değil kullanıcının bulunduğu sayfaya geri dönüyoruz
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KoyuModDegistir(string? donusUrl)
    {
        var kayitliDeger = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.KoyuMod);
        var suankiKoyuMu = bool.TryParse(kayitliDeger, out var deger) && deger;
        await _ayarServisi.DegerAtaAsync(AyarAnahtarlari.KoyuMod, (!suankiKoyuMu).ToString());

        // donusUrl kullanıcı girdisi olduğu için sitenin dışına yönlendirme yapılmasın diye yerel olduğu doğrulanıyor
        if (!string.IsNullOrWhiteSpace(donusUrl) && Url.IsLocalUrl(donusUrl))
        {
            return Redirect(donusUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    // geçici: arkadaşlara test amaçlı gönderirken bir tuşla örnek veri yükleyebilmek için, üst çubuktaki düğmeler burayı çağırıyor
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DemoVeriYukle(string? donusUrl)
    {
        var sonuc = await _demoVeriServisi.YukleAsync();
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Test verisi yüklendi."
            : string.Join(" ", sonuc.HataMesajlari);

        if (!string.IsNullOrWhiteSpace(donusUrl) && Url.IsLocalUrl(donusUrl))
        {
            return Redirect(donusUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    // geçici: yukarıdaki düğmenin yanında duran test verisini silme düğmesi burayı çağırıyor
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DemoVeriSil(string? donusUrl)
    {
        var sonuc = await _demoVeriServisi.SilAsync();
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Test verisi silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        if (!string.IsNullOrWhiteSpace(donusUrl) && Url.IsLocalUrl(donusUrl))
        {
            return Redirect(donusUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> YedekAl()
    {
        var yedekKlasoru = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.YedekKlasoru);
        if (string.IsNullOrWhiteSpace(yedekKlasoru))
        {
            TempData["HataMesaji"] = "Yedek almadan önce yedekleme klasörünü kaydedin.";
            return RedirectToAction(nameof(Index));
        }

        var sonuc = await _yedeklemeServisi.YedekAlAsync(yedekKlasoru);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Yedek alındı."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> YedektenGeriYukle(string yedekDosyaYolu)
    {
        var sonuc = await _yedeklemeServisi.YedektenGeriYukleAsync(yedekDosyaYolu);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Veritabanı seçilen yedekten geri yüklendi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    private async Task<AyarlarViewModel> ModelOlusturAsync()
    {
        var yedekKlasoru = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.YedekKlasoru);
        var otomatikYedekDegeri = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.KapanistaOtomatikYedek);
        var kayitliTema = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.SeciliTema);

        return new AyarlarViewModel
        {
            KurumAdi = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.KurumAdi),
            OgretmenAdi = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.OgretmenAdi),
            YedekKlasoru = yedekKlasoru,
            KapanistaOtomatikYedek = bool.TryParse(otomatikYedekDegeri, out var deger) && deger,
            // kayıtlı değer bozuksa ya da hiç yoksa varsayılan temaya düşüyoruz
            SeciliTema = TemaSecenekleri.GecerliMi(kayitliTema) ? kayitliTema! : TemaSecenekleri.Varsayilan,
            Yedekler = string.IsNullOrWhiteSpace(yedekKlasoru) ? [] : await _yedeklemeServisi.YedekleriListeleAsync(yedekKlasoru)
        };
    }
}
