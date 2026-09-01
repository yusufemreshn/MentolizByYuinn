using Microsoft.AspNetCore.Mvc;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Business.Dto.Tercih;
using Mentoliz.Web.Helpers;
using Mentoliz.Web.ViewModels;

namespace Mentoliz.Web.Controllers;

public class OgrencilerController : Controller
{
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly ISinifServisi _sinifServisi;
    private readonly IDersServisi _dersServisi;
    private readonly IOgrenciProgramServisi _ogrenciProgramServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IHedefServisi _hedefServisi;
    private readonly ICalismaProgramiServisi _calismaProgramiServisi;
    private readonly IKonuServisi _konuServisi;
    private readonly IVeliIletisimServisi _veliIletisimServisi;
    private readonly IZamanTuneliServisi _zamanTuneliServisi;
    private readonly IDonemDegerlendirmeServisi _donemDegerlendirmeServisi;
    private readonly IOgrenciTercihiServisi _ogrenciTercihiServisi;

    // zaman tüneli sekmesinde en fazla bu kadar kayıt gösteriliyor
    private const int ZamanTuneliAdedi = 40;

    // dönem raporunda tarih aralığı seçilmezse bugünden geriye doğru bu kadar ay gösteriliyor
    private const int DonemVarsayilanAySayisi = 4;

    public OgrencilerController(
        IOgrenciServisi ogrenciServisi,
        ISinifServisi sinifServisi,
        IDersServisi dersServisi,
        IOgrenciProgramServisi ogrenciProgramServisi,
        IOdevServisi odevServisi,
        IDenemeAnaliziServisi denemeAnaliziServisi,
        IGorusmeServisi gorusmeServisi,
        IHedefServisi hedefServisi,
        ICalismaProgramiServisi calismaProgramiServisi,
        IKonuServisi konuServisi,
        IVeliIletisimServisi veliIletisimServisi,
        IZamanTuneliServisi zamanTuneliServisi,
        IDonemDegerlendirmeServisi donemDegerlendirmeServisi,
        IOgrenciTercihiServisi ogrenciTercihiServisi)
    {
        _ogrenciServisi = ogrenciServisi;
        _sinifServisi = sinifServisi;
        _dersServisi = dersServisi;
        _ogrenciProgramServisi = ogrenciProgramServisi;
        _odevServisi = odevServisi;
        _denemeAnaliziServisi = denemeAnaliziServisi;
        _gorusmeServisi = gorusmeServisi;
        _hedefServisi = hedefServisi;
        _calismaProgramiServisi = calismaProgramiServisi;
        _konuServisi = konuServisi;
        _veliIletisimServisi = veliIletisimServisi;
        _zamanTuneliServisi = zamanTuneliServisi;
        _donemDegerlendirmeServisi = donemDegerlendirmeServisi;
        _ogrenciTercihiServisi = ogrenciTercihiServisi;
    }

    public async Task<IActionResult> Index(string? arama, int? sinifId, bool? aktifMi)
    {
        var filtre = new OgrenciFiltreDTO { Arama = arama, SinifId = sinifId, AktifMi = aktifMi };

        var model = new OgrenciListeViewModel
        {
            Ogrenciler = await _ogrenciServisi.ListeleAsync(filtre),
            SinifSecenekleri = await _sinifServisi.ListeleAsync(),
            Arama = arama,
            SinifId = sinifId,
            AktifMi = aktifMi
        };

        return View(model);
    }

    public async Task<IActionResult> Detay(int id, int? sinavTuruId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(id);
        if (ogrenci is null)
        {
            return NotFound();
        }

        // adreste sınav türü belirtilmemişse öğrencinin katıldığı ilk sınav türü sekmesi otomatik açılıyor
        var katildigiSinavTurleri = await _denemeAnaliziServisi.OgrenciKatildigiSinavTurleriniListeleAsync(id);
        var seciliSinavTuruId = sinavTuruId ?? katildigiSinavTurleri.FirstOrDefault()?.Id;

        // detay sayfası dokuz sekmeyi tek seferde dolduruyor, her sekme kendi servisinden veri çekiyor
        var model = new OgrenciDetayViewModel
        {
            Ogrenci = ogrenci,
            BirlesikProgram = await _ogrenciProgramServisi.BirlesikProgramiHesaplaAsync(id),
            HaftalikBulunmaSuresi = await _ogrenciProgramServisi.HaftalikBulunmaSuresiHesaplaAsync(id),
            Istisnalar = await _ogrenciProgramServisi.IstisnalariListeleAsync(id),
            Odevler = await _odevServisi.ListeleAsync(new OdevFiltreDTO { OgrenciId = id }),
            OdevOzeti = await _odevServisi.OgrenciOzetiHesaplaAsync(id),
            KatildigiSinavTurleri = katildigiSinavTurleri,
            SeciliSinavTuruId = seciliSinavTuruId,
            DenemeAnalizi = seciliSinavTuruId.HasValue
                ? await _denemeAnaliziServisi.AnaliziHesaplaAsync(id, seciliSinavTuruId.Value)
                : new(),
            Gorusmeler = await _gorusmeServisi.ListeleAsync(new GorusmeFiltreDTO { OgrenciId = id }),
            AktifHedef = await _hedefServisi.AktifHedefGetirAsync(id),
            HedefKarsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(id),
            GuncelCalismaProgrami = (await _calismaProgramiServisi.ListeleAsync(id)).FirstOrDefault(),
            KonuIlerlemeleri = await _konuServisi.DersBazliIlerlemeHesaplaAsync(id),
            ZamanTuneli = await _zamanTuneliServisi.OlusturAsync(id, ZamanTuneliAdedi),
            ZayifKonuOnerileri = await _konuServisi.ZayifKonuOnerileriHesaplaAsync(id),
            Tercihler = await _ogrenciTercihiServisi.ListeleAsync(id)
        };

        return View(model);
    }

    public async Task<IActionResult> CsvDisaAktar(string? arama, int? sinifId, bool? aktifMi)
    {
        // listede uygulanan aynı filtreyle dışa aktarılıyor, ekranda ne görülüyorsa csv'ye de o iniyor
        var filtre = new OgrenciFiltreDTO { Arama = arama, SinifId = sinifId, AktifMi = aktifMi };
        var ogrenciler = await _ogrenciServisi.ListeleAsync(filtre);

        var basliklar = new[] { "Ad Soyad", "Öğrenci No", "Sınıf", "Telefon", "Kayıt Tarihi", "Durum" };
        var satirlar = ogrenciler.Select(o => new[]
        {
            o.AdSoyad,
            o.OgrenciNo ?? "",
            o.SinifAdi ?? "",
            o.Telefon ?? "",
            o.KayitTarihi.ToString("dd.MM.yyyy"),
            o.AktifMi ? "Aktif" : "Ayrılmış"
        });

        return CsvYazici.OlusturDosya(basliklar, satirlar, $"ogrenciler-{DateTime.Now:yyyyMMdd-HHmm}.csv");
    }

    public async Task<IActionResult> OgretmenRaporu(int id)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(id);
        if (ogrenci is null)
        {
            return NotFound();
        }

        // raporda tek sınav türü gösteriliyor, öğrencinin katıldığı ilk tür seçiliyor
        var katildigiSinavTurleri = await _denemeAnaliziServisi.OgrenciKatildigiSinavTurleriniListeleAsync(id);
        var seciliSinavTuruId = katildigiSinavTurleri.FirstOrDefault()?.Id;

        var model = new OgretmenRaporuViewModel
        {
            Ogrenci = ogrenci,
            DenemeAnalizi = seciliSinavTuruId.HasValue
                ? await _denemeAnaliziServisi.AnaliziHesaplaAsync(id, seciliSinavTuruId.Value)
                : new(),
            OdevOzeti = await _odevServisi.OgrenciOzetiHesaplaAsync(id),
            KonuIlerlemeleri = await _konuServisi.DersBazliIlerlemeHesaplaAsync(id),
            HedefKarsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(id),
            Gorusmeler = await _gorusmeServisi.ListeleAsync(new GorusmeFiltreDTO { OgrenciId = id }),
            OlusturmaTarihi = DateTime.Now
        };

        return View(model);
    }

    public async Task<IActionResult> VeliRaporu(int id)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(id);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var katildigiSinavTurleri = await _denemeAnaliziServisi.OgrenciKatildigiSinavTurleriniListeleAsync(id);
        var seciliSinavTuruId = katildigiSinavTurleri.FirstOrDefault()?.Id;

        var model = new VeliRaporuViewModel
        {
            Ogrenci = ogrenci,
            DenemeAnalizi = seciliSinavTuruId.HasValue
                ? await _denemeAnaliziServisi.AnaliziHesaplaAsync(id, seciliSinavTuruId.Value)
                : new(),
            OdevOzeti = await _odevServisi.OgrenciOzetiHesaplaAsync(id),
            HaftalikBulunmaSuresi = await _ogrenciProgramServisi.HaftalikBulunmaSuresiHesaplaAsync(id),
            OlusturmaTarihi = DateTime.Now,
            VeliMesajiMetni = await _veliIletisimServisi.MesajOlusturAsync(id)
        };

        return View(model);
    }

    public async Task<IActionResult> DonemRaporu(int id, DateTime? baslangic, DateTime? bitis)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(id);
        if (ogrenci is null)
        {
            return NotFound();
        }

        // aralık belirtilmemişse bugünden geriye doğru varsayılan dönem uzunluğu kullanılıyor
        var donemBitis = bitis ?? DateTime.Today;
        var donemBaslangic = baslangic ?? donemBitis.AddMonths(-DonemVarsayilanAySayisi);

        var model = new DonemRaporuViewModel
        {
            Ogrenci = ogrenci,
            Degerlendirme = await _donemDegerlendirmeServisi.OlusturAsync(id, donemBaslangic, donemBitis),
            OlusturmaTarihi = DateTime.Now
        };

        return View(model);
    }

    public async Task<IActionResult> Ekle()
    {
        var model = new OgrenciFormViewModel
        {
            Ogrenci = new OgrenciKaydetDTO { KayitTarihi = DateTime.Today, AktifMi = true },
            SinifSecenekleri = await _sinifServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(OgrenciKaydetDTO ogrenci)
    {
        var sonuc = await _ogrenciServisi.EkleAsync(ogrenci);
        if (!sonuc.Basarili)
        {
            // doğrulama hatası varsa kullanıcının girdiği veriyi kaybetmeden aynı formu tekrar gösteriyoruz
            var model = new OgrenciFormViewModel
            {
                Ogrenci = ogrenci,
                SinifSecenekleri = await _sinifServisi.ListeleAsync()
            };
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(model);
        }

        TempData["BasariMesaji"] = "Öğrenci başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var ogrenci = await _ogrenciServisi.DuzenlemeIcinGetirAsync(id);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var model = new OgrenciFormViewModel
        {
            Ogrenci = ogrenci,
            SinifSecenekleri = await _sinifServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(OgrenciKaydetDTO ogrenci)
    {
        var sonuc = await _ogrenciServisi.GuncelleAsync(ogrenci);
        if (!sonuc.Basarili)
        {
            var model = new OgrenciFormViewModel
            {
                Ogrenci = ogrenci,
                SinifSecenekleri = await _sinifServisi.ListeleAsync()
            };
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(model);
        }

        TempData["BasariMesaji"] = "Öğrenci başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _ogrenciServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Öğrenci silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> IstisnaEkle(int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var model = new IstisnaFormViewModel
        {
            Istisna = new OgrenciProgramIstisnasiKaydetDTO { OgrenciId = ogrenciId },
            OgrenciAdSoyad = ogrenci.AdSoyad,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IstisnaEkle(OgrenciProgramIstisnasiKaydetDTO istisna)
    {
        var sonuc = await _ogrenciProgramServisi.IstisnaEkleAsync(istisna);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(istisna.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new IstisnaFormViewModel
            {
                Istisna = istisna,
                OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Program istisnası eklendi.";
        return RedirectToAction(nameof(Detay), new { id = istisna.OgrenciId });
    }

    public async Task<IActionResult> IstisnaDuzenle(int id)
    {
        var istisna = await _ogrenciProgramServisi.IstisnaDuzenlemeIcinGetirAsync(id);
        if (istisna is null)
        {
            return NotFound();
        }

        var ogrenci = await _ogrenciServisi.TekGetirAsync(istisna.OgrenciId);

        var model = new IstisnaFormViewModel
        {
            Istisna = istisna,
            OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
            DersSecenekleri = await _dersServisi.ListeleAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IstisnaDuzenle(OgrenciProgramIstisnasiKaydetDTO istisna)
    {
        var sonuc = await _ogrenciProgramServisi.IstisnaGuncelleAsync(istisna);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(istisna.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new IstisnaFormViewModel
            {
                Istisna = istisna,
                OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty,
                DersSecenekleri = await _dersServisi.ListeleAsync()
            });
        }

        TempData["BasariMesaji"] = "Program istisnası güncellendi.";
        return RedirectToAction(nameof(Detay), new { id = istisna.OgrenciId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IstisnaSil(int id, int ogrenciId)
    {
        var sonuc = await _ogrenciProgramServisi.IstisnaSilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Program istisnası silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = ogrenciId });
    }

    public async Task<IActionResult> TercihEkle(int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return NotFound();
        }

        var mevcutTercihSayisi = (await _ogrenciTercihiServisi.ListeleAsync(ogrenciId)).Count;

        var model = new TercihFormViewModel
        {
            // yeni tercih eklenirken bir sonraki sıra numarası otomatik öneriliyor
            Tercih = new OgrenciTercihiKaydetDTO { OgrenciId = ogrenciId, Sira = mevcutTercihSayisi + 1 },
            OgrenciAdSoyad = ogrenci.AdSoyad
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TercihEkle(OgrenciTercihiKaydetDTO tercih)
    {
        var sonuc = await _ogrenciTercihiServisi.EkleAsync(tercih);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(tercih.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new TercihFormViewModel { Tercih = tercih, OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty });
        }

        TempData["BasariMesaji"] = "Tercih eklendi.";
        return RedirectToAction(nameof(Detay), new { id = tercih.OgrenciId });
    }

    public async Task<IActionResult> TercihDuzenle(int id)
    {
        var tercih = await _ogrenciTercihiServisi.DuzenlemeIcinGetirAsync(id);
        if (tercih is null)
        {
            return NotFound();
        }

        var ogrenci = await _ogrenciServisi.TekGetirAsync(tercih.OgrenciId);

        var model = new TercihFormViewModel
        {
            Tercih = tercih,
            OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TercihDuzenle(OgrenciTercihiKaydetDTO tercih)
    {
        var sonuc = await _ogrenciTercihiServisi.GuncelleAsync(tercih);
        if (!sonuc.Basarili)
        {
            var ogrenci = await _ogrenciServisi.TekGetirAsync(tercih.OgrenciId);
            TempData["HataMesaji"] = string.Join(" ", sonuc.HataMesajlari);
            return View(new TercihFormViewModel { Tercih = tercih, OgrenciAdSoyad = ogrenci?.AdSoyad ?? string.Empty });
        }

        TempData["BasariMesaji"] = "Tercih güncellendi.";
        return RedirectToAction(nameof(Detay), new { id = tercih.OgrenciId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TercihSil(int id, int ogrenciId)
    {
        var sonuc = await _ogrenciTercihiServisi.SilAsync(id);
        TempData[sonuc.Basarili ? "BasariMesaji" : "HataMesaji"] = sonuc.Basarili
            ? "Tercih silindi."
            : string.Join(" ", sonuc.HataMesajlari);

        return RedirectToAction(nameof(Detay), new { id = ogrenciId });
    }
}
