using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

// günün görevlerini, bugün teslim olan ödevleri ve ihmal edilen öğrencileri tek ekranda toplayan çalışma sayfası
public class BugunController : Controller
{
    // bu gün sayısından uzun süredir görüşülmeyenler burada "ihmal edilen" sayılıyor, ana sayfadaki otuz günlük genel uyarıdan daha sıkı bir eşik
    private const int IhmalEsigiGunSayisi = 60;

    private readonly IGorevServisi _gorevServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IGorusmeServisi _gorusmeServisi;

    public BugunController(IGorevServisi gorevServisi, IOdevServisi odevServisi, IGorusmeServisi gorusmeServisi)
    {
        _gorevServisi = gorevServisi;
        _odevServisi = odevServisi;
        _gorusmeServisi = gorusmeServisi;
    }

    public async Task<IActionResult> Index()
    {
        var tumOdevler = await _odevServisi.ListeleAsync();
        var uzunSureGorusulmeyenler = await _gorusmeServisi.UzunSureGorusulmeyenleriListeleAsync();

        var model = new BugunIndexViewModel
        {
            BugunkuGorevler = await _gorevServisi.BugunkuGorevleriListeleAsync(),
            BugunTeslimOdevler = tumOdevler.Where(o => o.SonTeslimTarihi.Date == DateTime.Today).ToList(),
            IhmalEdilenOgrenciler = uzunSureGorusulmeyenler
                .Where(o => o.GecenGunSayisi is null || o.GecenGunSayisi >= IhmalEsigiGunSayisi)
                .ToList()
        };

        return View(model);
    }
}
