using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class HomeController : Controller
{
    // ana sayfadaki yaklaşan teslim tarihleri listesinde en fazla bu kadar ödev gösteriliyor
    private const int YaklasanTeslimTarihiAdedi = 5;

    // dikkat gerektiren öğrenciler listesinde en fazla bu kadar öğrenci gösteriliyor
    private const int DikkatListesiAdedi = 8;

    // bu puanın altındaki öğrenciler dikkat listesinde hiç görünmüyor, gürültü olmasın diye
    private const int DikkatListesiMinimumPuan = 20;

    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IGorevServisi _gorevServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IDenemeServisi _denemeServisi;
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;
    private readonly IRiskDegerlendirmeServisi _riskDegerlendirmeServisi;

    public HomeController(
        IOgrenciServisi ogrenciServisi,
        IGorevServisi gorevServisi,
        IOdevServisi odevServisi,
        IDenemeServisi denemeServisi,
        IGorusmeServisi gorusmeServisi,
        IDenemeAnaliziServisi denemeAnaliziServisi,
        IRiskDegerlendirmeServisi riskDegerlendirmeServisi)
    {
        _ogrenciServisi = ogrenciServisi;
        _gorevServisi = gorevServisi;
        _odevServisi = odevServisi;
        _denemeServisi = denemeServisi;
        _gorusmeServisi = gorusmeServisi;
        _denemeAnaliziServisi = denemeAnaliziServisi;
        _riskDegerlendirmeServisi = riskDegerlendirmeServisi;
    }

    public async Task<IActionResult> Index()
    {
        var aktifOgrenciler = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true });
        var bugunkuGorevler = await _gorevServisi.BugunkuGorevleriListeleAsync();
        var gecikenOdevler = await _odevServisi.ListeleAsync(new OdevFiltreDTO { Durum = OdevDurumu.Gecikti });

        // risk puanları hesaplanır hesaplanmaz yüksek riskli öğrenciler için otomatik görev açılıyor, aynı görev tekrar açılmıyor
        var riskListesi = await _riskDegerlendirmeServisi.DegerlendirAsync();
        await _riskDegerlendirmeServisi.OtomatikGorevOlusturAsync(riskListesi);

        var model = new HomeIndexViewModel
        {
            ToplamOgrenciSayisi = aktifOgrenciler.Count,
            BugunGorusulecekSayisi = bugunkuGorevler.Count,
            GecikenOdevSayisi = gecikenOdevler.Count,
            BuHaftaGirilenDenemeSayisi = await _denemeServisi.BuHaftaGirilenSayisiniHesaplaAsync(),
            HaftalikOdevTamamlamaOrani = await _odevServisi.HaftalikTamamlamaOraniHesaplaAsync(),
            YaklasanTeslimTarihleri = await _odevServisi.YaklasanTeslimTarihleriniListeleAsync(YaklasanTeslimTarihiAdedi),
            BugunkuGorevler = bugunkuGorevler,
            UzunSureGorusulmeyenler = await _gorusmeServisi.UzunSureGorusulmeyenleriListeleAsync(),
            SinifOrtalamaNetSonucu = await _denemeAnaliziServisi.SinifBazliOrtalamaNetleriHesaplaAsync(),
            SonDenemeNetDegisimOzeti = await _denemeAnaliziServisi.SonDenemeNetDegisimOzetiHesaplaAsync(),
            DikkatGerektirenOgrenciler = riskListesi.Where(r => r.RiskPuani >= DikkatListesiMinimumPuan).Take(DikkatListesiAdedi).ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
