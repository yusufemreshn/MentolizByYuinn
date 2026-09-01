using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Context;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

// arkadaşlara veya tanıtım için programı gönderirken bir tuşla zengin örnek veri yükleyip silebilmek için yazıldı, teslimden önce kaldırılacak geçici bir özellik
public class DemoVeriServisi : IDemoVeriServisi
{
    // baslangicverisi.cs ile migration'a gömülü sabit ders ve sınav türü kimlikleri, burada tekrar oluşturmuyoruz sadece referans veriyoruz
    private const int DersTurkce = 1, DersMatematik = 2, DersSosyal = 3, DersFen = 4, DersFizik = 5, DersKimya = 6, DersBiyoloji = 7,
        DersTde = 8, DersTarih = 9, DersCografya = 10, DersTarih2 = 11, DersCografya2 = 12, DersFelsefe = 13, DersDin = 14, DersYabanciDil = 15;

    private const int SinavTuruTyt = 1, SinavTuruAytSayisal = 2, SinavTuruAytEsitAgirlik = 3, SinavTuruAytSozel = 4;

    private static readonly (int SinavTuruId, int TestId, int DersId, int SoruSayisi)[] TestTanimlari =
    [
        (SinavTuruTyt, 1, DersTurkce, 40), (SinavTuruTyt, 2, DersMatematik, 40), (SinavTuruTyt, 3, DersSosyal, 20), (SinavTuruTyt, 4, DersFen, 20),
        (SinavTuruAytSayisal, 5, DersMatematik, 40), (SinavTuruAytSayisal, 6, DersFizik, 14), (SinavTuruAytSayisal, 7, DersKimya, 13), (SinavTuruAytSayisal, 8, DersBiyoloji, 13),
        (SinavTuruAytEsitAgirlik, 9, DersMatematik, 40), (SinavTuruAytEsitAgirlik, 10, DersTde, 24), (SinavTuruAytEsitAgirlik, 11, DersTarih, 10), (SinavTuruAytEsitAgirlik, 12, DersCografya, 6),
        (SinavTuruAytSozel, 13, DersTde, 24), (SinavTuruAytSozel, 14, DersTarih, 10), (SinavTuruAytSozel, 15, DersCografya, 6),
        (SinavTuruAytSozel, 16, DersTarih2, 11), (SinavTuruAytSozel, 17, DersCografya2, 11), (SinavTuruAytSozel, 18, DersFelsefe, 12), (SinavTuruAytSozel, 19, DersDin, 6)
    ];

    private static readonly (string Ad, Alan Alan, SinifSeviyesi Seviye)[] SinifTanimlari =
    [
        ("9-A", Alan.Sayisal, SinifSeviyesi.Dokuz),
        ("9-B", Alan.Sayisal, SinifSeviyesi.Dokuz),
        ("10-A", Alan.Sayisal, SinifSeviyesi.On),
        ("10-B", Alan.EsitAgirlik, SinifSeviyesi.On),
        ("11-Sayısal", Alan.Sayisal, SinifSeviyesi.OnBir),
        ("11-Eşit Ağırlık", Alan.EsitAgirlik, SinifSeviyesi.OnBir),
        ("11-Sözel", Alan.Sozel, SinifSeviyesi.OnBir),
        ("12-Sayısal", Alan.Sayisal, SinifSeviyesi.OnIki),
        ("12-Eşit Ağırlık", Alan.EsitAgirlik, SinifSeviyesi.OnIki),
        ("12-Sözel", Alan.Sozel, SinifSeviyesi.OnIki),
        ("Dil Hazırlık", Alan.Dil, SinifSeviyesi.Dokuz),
        ("Mezun Sayısal", Alan.Sayisal, SinifSeviyesi.Mezun)
    ];

    private static readonly string[] ErkekAdlari =
        ["Arda", "Kaan", "Efe", "Berat", "Bora", "Cem", "Deniz", "Emir", "Kerem", "Umut", "Volkan", "Serkan", "Onur", "Baran", "Kağan", "Tuna", "Yusuf", "Mert", "Alp", "Eren"];

    private static readonly string[] KizAdlari =
        ["Ada", "Defne", "Ecrin", "Elif", "Ceren", "Beren", "Zeynep", "Nisa", "Sude", "Pelin", "Irmak", "Ebru", "Naz", "Selin", "Yaren", "Melis", "Duru", "İpek", "Gizem", "Aslı"];

    private static readonly string[] Soyadlar =
        ["Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Yıldırım", "Öztürk", "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin", "Koç", "Kurt", "Şen", "Bulut", "Toprak", "Karaca", "Turan", "Tunç", "Sarı"];

    private static readonly string[] Okullar =
        ["Cumhuriyet Anadolu Lisesi", "Mimar Sinan Anadolu Lisesi", "Fatih Fen Lisesi", "Atatürk Anadolu Lisesi", "Gazi Anadolu Lisesi", "İstiklal Fen Lisesi", "Yüzyıl Anadolu Lisesi", "Barış Anadolu Lisesi"];

    private static readonly string[] VeliMeslekleri =
        ["Öğretmen", "Mühendis", "Doktor", "Avukat", "Esnaf", "Muhasebeci", "Hemşire", "Serbest Meslek", "Memur", "Ev Hanımı"];

    private static readonly string[] OgrenciNotPool =
        ["çalışma disiplini iyi, düzenli takip ediliyor", "motivasyonu düşük, yakın takip gerekiyor", "sınav kaygısı var, rahatlatıcı konuşmalar yapıldı", "hedefi net, planlı ilerliyor", "velisiyle iletişim güçlü", "devamsızlık takibi yapılıyor"];

    private static readonly (Alan Alan, string Universite, string Bolum)[] HedefPool =
    [
        (Alan.Sayisal, "Boğaziçi Üniversitesi", "Bilgisayar Mühendisliği"),
        (Alan.Sayisal, "ODTÜ", "Elektrik-Elektronik Mühendisliği"),
        (Alan.Sayisal, "Hacettepe Üniversitesi", "Tıp Fakültesi"),
        (Alan.Sayisal, "Yıldız Teknik Üniversitesi", "Endüstri Mühendisliği"),
        (Alan.EsitAgirlik, "Ankara Üniversitesi", "Hukuk Fakültesi"),
        (Alan.EsitAgirlik, "İstanbul Üniversitesi", "İşletme"),
        (Alan.EsitAgirlik, "Gazi Üniversitesi", "Öğretmenlik"),
        (Alan.Sozel, "İstanbul Üniversitesi", "Türk Dili ve Edebiyatı"),
        (Alan.Sozel, "Ankara Üniversitesi", "Tarih"),
        (Alan.Dil, "Boğaziçi Üniversitesi", "Mütercim Tercümanlık")
    ];

    private static readonly string[] GorusmeKonuPool =
        ["Ders çalışma planı", "Motivasyon görüşmesi", "Sınav kaygısı", "Veli bilgilendirme", "Hedef üniversite planlaması", "Devamsızlık görüşmesi"];

    private static readonly string[] GorusmeNotPool =
        ["öğrenciyle haftalık çalışma programı üzerinden konuşuldu, uyumlu ilerliyor", "sınav kaygısını azaltmak için nefes teknikleri önerildi", "aile içi beklenti yüksek, öğrenciyle bu konuda konuşuldu", "hedefler gözden geçirildi, küçük revizyonlar yapıldı"];

    private static readonly string[] GorevBaslikPool =
        ["Veli görüşmesi yap", "Deneme sonuçlarını değerlendir", "Ders programını güncelle", "Konu takip çizelgesini kontrol et", "Ödev takibini kontrol et", "Yeni dönem planlaması yap"];

    private static readonly (string Baslik, string Icerik)[] NotPool =
    [
        ("Veli toplantısı hazırlığı", "Bu dönemki veli toplantısında sınıf bazlı net ortalamalarını ve devamsızlık durumunu sunacağım, grafik çıktıları hazırlanacak."),
        ("Sınav kaygısı kaynakları", "Sınav kaygısıyla ilgili okuduğum makalelerde nefes teknikleri ve zaman yönetimi öne çıkıyor, öğrencilerle paylaşılacak."),
        ("Yeni dönem hedef görüşmeleri", "Şubat ayı başında tüm öğrencilerle hedef güncelleme görüşmesi yapılacak, takvim çıkarılacak."),
        ("Kaynak kitap önerileri", "Sayısal alanda zorlanan öğrenciler için ek soru bankası önerileri not edildi, dönem sonunda paylaşılacak."),
        ("Devamsızlık uyarı sistemi", "Art arda üç gün devamsızlık yapan öğrenciler için otomatik hatırlatma fikri, ileride görev modülüne eklenebilir."),
        ("Rehberlik seminer notları", "Katıldığım seminerde ergen motivasyonu üzerine alınan notlar, uygun görüşmelerde kullanılacak."),
        ("Sınıf öğretmenleriyle koordinasyon", "Her ayın son haftası sınıf öğretmenleriyle kısa bir değerlendirme toplantısı yapılması planlandı."),
        ("Başarı hikayeleri", "Geçen yıl hedeflerine ulaşan öğrencilerin kısa hikayeleri, motivasyon amaçlı yeni öğrencilerle paylaşılabilir."),
        ("Zaman yönetimi atölyesi fikri", "Sınava hazırlanan öğrenciler için kısa bir zaman yönetimi atölyesi düzenlenebilir, içerik taslağı çıkarılacak."),
        ("Veli iletişim şablonu", "Sık kullanılan veli bilgilendirme mesajları için birkaç şablon cümle hazırlandı."),
        ("Konu tekrar önerisi", "Deneme sonuçlarında sürekli düşük çıkan konular için haftalık kısa tekrar listesi hazırlanabilir."),
        ("Mezun öğrenci takibi", "Geçen yıl mezun olan birkaç öğrenciyle iletişim kurup üniversite süreçlerini sormak istiyorum."),
        ("Kütüphane işbirliği", "Okul kütüphanesiyle görüşüp sınava hazırlık kitapları için ayrı bir raf oluşturulması istendi."),
        ("Bireysel farklar notu", "Bazı öğrenciler grup çalışmasında bazıları bireysel çalışmada daha verimli, programlarken bunu göz önünde tutmalıyım."),
        ("Sınav sonrası değerlendirme rutini", "Her denemeden sonraki hafta kısa birebir değerlendirme görüşmesi yapılması faydalı oluyor, devam edilecek."),
        ("Yeni konu takip fikri", "Konu takip çizelgesine zorluk seviyesine göre renklendirme eklenirse öğrenciler için daha anlaşılır olabilir."),
        ("Uzun süredir görüşülmeyenler", "Bazı öğrencilerle son bir aydır görüşme yapılamadı, önümüzdeki hafta öncelik verilecek."),
        ("Motivasyon konuşması notları", "Sınav öncesi yapılan grup motivasyon konuşmasında kullanılan örnekler not edildi."),
        ("Program revizyon fikri", "Çalışma programı şablonuna haftalık kısa bir öz değerlendirme sorusu eklenebilir."),
        ("Genel gözlem", "Bu dönem genel olarak devamsızlık geçen döneme göre azaldı, veli bilgilendirmelerinin etkili olduğunu düşünüyorum.")
    ];

    private readonly MentolizDbContext _baglam;
    private readonly Random _rastgele = new();

    public DemoVeriServisi(MentolizDbContext baglam)
    {
        _baglam = baglam;
    }

    public async Task<IslemSonucu> SilAsync()
    {
        try
        {
            await TemizleAsync();
            return IslemSonucu.Basar();
        }
        catch (Exception hata)
        {
            return IslemSonucu.Basarisiz($"Test verisi silinemedi: {hata.Message}");
        }
    }

    public async Task<IslemSonucu> YukleAsync()
    {
        try
        {
            await TemizleAsync();
            Uret();
            await _baglam.SaveChangesAsync();
            return IslemSonucu.Basar();
        }
        catch (Exception hata)
        {
            return IslemSonucu.Basarisiz($"Test verisi yüklenemedi: {hata.Message}");
        }
    }

    // müfredat tablolarına (dersler, konular, sınav türleri, testleri) ve ayarlara dokunmadan geri kalan işletme verisini komple temizliyor
    private async Task TemizleAsync()
    {
        string[] tablolar =
        [
            "DenemeSonucDetaylari", "DenemeSonuclari", "Denemeler",
            "Odevler", "OdevTopluAtamalari",
            "Gorusmeler",
            "HedefDersNetleri", "Hedefler",
            "CalismaProgramiSatirlari", "CalismaProgramlari",
            "OgrenciKonuTakipleri",
            "Gorevler",
            "OgrenciProgramIstisnalari", "DersProgramiSatirlari",
            "Veliler", "Ogrenciler", "Siniflar",
            "Notlar"
        ];

        foreach (var tablo in tablolar)
        {
            // tablo adı yukarıdaki sabit dahili listeden geliyor, dışarıdan kullanıcı girdisi değil, o yüzden ef1002 uyarısı burada güvenle kapatılıyor
#pragma warning disable EF1002
            await _baglam.Database.ExecuteSqlRawAsync($"DELETE FROM \"{tablo}\";");
#pragma warning restore EF1002
            await _baglam.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = {0};", tablo);
        }
    }

    private void Uret()
    {
        var siniflar = SiniflariOlustur();
        var ogrenciler = OgrencileriOlustur(siniflar);

        DenemeleriOlustur(ogrenciler);
        OdevleriOlustur(ogrenciler);
        GorusmeleriOlustur(ogrenciler);
        HedefleriOlustur(ogrenciler);
        CalismaProgramlariniOlustur(ogrenciler);
        KonuTakipleriniOlustur(ogrenciler);
        GorevleriOlustur(ogrenciler);
        NotlariOlustur();
    }

    private List<Sinif> SiniflariOlustur()
    {
        var siniflar = SinifTanimlari.Select(t => new Sinif
        {
            Ad = t.Ad,
            Alan = t.Alan,
            Seviye = t.Seviye,
            Kontenjan = 20,
            AktifMi = true
        }).ToList();

        foreach (var sinif in siniflar)
        {
            // sınıfın alanına uygun küçük bir ders havuzuyla basit bir haftalık program kuruyoruz, beş gün iki saat
            var dersHavuzu = DersHavuzu(sinif.Alan);
            var gunler = new[] { Gun.Pazartesi, Gun.Sali, Gun.Carsamba, Gun.Persembe, Gun.Cuma };
            var saatler = new[] { (Baslangic: new TimeSpan(9, 0, 0), Bitis: new TimeSpan(9, 50, 0)), (Baslangic: new TimeSpan(10, 0, 0), Bitis: new TimeSpan(10, 50, 0)) };

            var sayac = 0;
            foreach (var gun in gunler)
            {
                foreach (var saat in saatler)
                {
                    sinif.DersProgramiSatirlari.Add(new DersProgramiSatiri
                    {
                        Sinif = sinif,
                        Gun = gun,
                        BaslangicSaati = saat.Baslangic,
                        BitisSaati = saat.Bitis,
                        DersId = dersHavuzu[sayac % dersHavuzu.Length]
                    });
                    sayac++;
                }
            }
        }

        _baglam.AddRange(siniflar);
        return siniflar;
    }

    private static int[] DersHavuzu(Alan alan) => alan switch
    {
        Alan.Sayisal => [DersMatematik, DersFizik, DersKimya, DersBiyoloji, DersTurkce],
        Alan.EsitAgirlik => [DersMatematik, DersTde, DersTarih, DersTurkce],
        Alan.Sozel => [DersTde, DersTarih, DersCografya, DersFelsefe],
        Alan.Dil => [DersYabanciDil, DersTurkce, DersMatematik],
        _ => [DersTurkce, DersMatematik]
    };

    private List<Ogrenci> OgrencileriOlustur(List<Sinif> siniflar)
    {
        var ogrenciler = new List<Ogrenci>();
        var ogrenciNoSayaci = 1;

        foreach (var sinif in siniflar)
        {
            // her sınıfa iki veya üç öğrenci düşecek şekilde dağıtıyoruz
            var ogrenciSayisi = _rastgele.Next(2, 4);

            for (var i = 0; i < ogrenciSayisi; i++)
            {
                var kizMi = _rastgele.Next(2) == 0;
                var ad = kizMi ? KizAdlari[_rastgele.Next(KizAdlari.Length)] : ErkekAdlari[_rastgele.Next(ErkekAdlari.Length)];
                var soyad = Soyadlar[_rastgele.Next(Soyadlar.Length)];
                var aktifMi = _rastgele.Next(10) > 0; // onda bir ihtimalle ayrılmış öğrenci

                var ogrenci = new Ogrenci
                {
                    Ad = ad,
                    Soyad = soyad,
                    Telefon = RastgeleTelefon(),
                    OgrenciNo = $"2026{ogrenciNoSayaci:000}",
                    Sinif = sinif,
                    DogumTarihi = DateTime.Today.AddYears(-(15 + (int)sinif.Seviye - 9)).AddDays(-_rastgele.Next(365)),
                    Cinsiyet = kizMi ? Cinsiyet.Kiz : Cinsiyet.Erkek,
                    OkulAdi = Okullar[_rastgele.Next(Okullar.Length)],
                    Alan = sinif.Alan,
                    KayitTarihi = DateTime.Today.AddDays(-_rastgele.Next(30, 365)),
                    AktifMi = aktifMi,
                    GenelNotlar = OgrenciNotPool[_rastgele.Next(OgrenciNotPool.Length)]
                };

                ogrenci.Veliler.Add(RastgeleVeliOlustur(ogrenci, Yakinlik.Anne, true));
                if (_rastgele.Next(2) == 0)
                {
                    ogrenci.Veliler.Add(RastgeleVeliOlustur(ogrenci, Yakinlik.Baba, false));
                }

                ogrenciler.Add(ogrenci);
                ogrenciNoSayaci++;
            }
        }

        _baglam.AddRange(ogrenciler);
        return ogrenciler;
    }

    private Veli RastgeleVeliOlustur(Ogrenci ogrenci, Yakinlik yakinlik, bool birincilMi) => new()
    {
        Ogrenci = ogrenci,
        Yakinlik = yakinlik,
        Ad = yakinlik == Yakinlik.Anne ? KizAdlari[_rastgele.Next(KizAdlari.Length)] : ErkekAdlari[_rastgele.Next(ErkekAdlari.Length)],
        Soyad = ogrenci.Soyad,
        Telefon = RastgeleTelefon(),
        Meslek = VeliMeslekleri[_rastgele.Next(VeliMeslekleri.Length)],
        Eposta = $"veli{_rastgele.Next(1000, 9999)}@ornek.com",
        BirincilIletisimMi = birincilMi
    };

    private string RastgeleTelefon() => $"05{_rastgele.Next(30, 55)} {_rastgele.Next(100, 999)} {_rastgele.Next(10, 99)} {_rastgele.Next(10, 99)}";

    private void DenemeleriOlustur(List<Ogrenci> ogrenciler)
    {
        var denemeler = new List<Deneme>();

        // herkesin girdiği dört tyt denemesi, son iki aya yayılmış
        for (var i = 0; i < 4; i++)
        {
            denemeler.Add(new Deneme
            {
                Ad = $"TYT Genel Deneme {i + 1}",
                Tarih = DateTime.Today.AddDays(-(60 - i * 15)),
                SinavTuruId = SinavTuruTyt,
                YayinAdi = "Karma Yayınları"
            });
        }

        // alanlarına uygun bir de ayt denemesi giren öğrenciler
        var aytTanimlari = new (int SinavTuruId, Alan Alan, string Ad)[]
        {
            (SinavTuruAytSayisal, Alan.Sayisal, "AYT Sayısal Deneme 1"),
            (SinavTuruAytEsitAgirlik, Alan.EsitAgirlik, "AYT Eşit Ağırlık Deneme 1"),
            (SinavTuruAytSozel, Alan.Sozel, "AYT Sözel Deneme 1")
        };

        foreach (var tanim in aytTanimlari)
        {
            denemeler.Add(new Deneme
            {
                Ad = tanim.Ad,
                Tarih = DateTime.Today.AddDays(-20),
                SinavTuruId = tanim.SinavTuruId,
                YayinAdi = "Karma Yayınları"
            });
        }

        _baglam.AddRange(denemeler);

        var aktifOgrenciler = ogrenciler.Where(o => o.AktifMi).ToList();

        foreach (var deneme in denemeler)
        {
            var testler = TestTanimlari.Where(t => t.SinavTuruId == deneme.SinavTuruId).ToList();

            // ayt denemelerine sadece o alandaki öğrenciler, tyt'ye herkes giriyor
            var katilimcilar = aytTanimlari.Any(a => a.SinavTuruId == deneme.SinavTuruId)
                ? aktifOgrenciler.Where(o => o.Alan == aytTanimlari.First(a => a.SinavTuruId == deneme.SinavTuruId).Alan).ToList()
                : aktifOgrenciler;

            foreach (var ogrenci in katilimcilar)
            {
                // her öğrenci denemeye girmiyor, biraz eksik katılım da gerçekçi olsun diye
                if (_rastgele.Next(10) == 0)
                {
                    continue;
                }

                var sonuc = new DenemeSonuc { Deneme = deneme, Ogrenci = ogrenci };

                foreach (var test in testler)
                {
                    var dogru = _rastgele.Next((int)(test.SoruSayisi * 0.3), (int)(test.SoruSayisi * 0.9) + 1);
                    var kalan = test.SoruSayisi - dogru;
                    var yanlis = kalan == 0 ? 0 : _rastgele.Next(0, kalan + 1);
                    var bos = test.SoruSayisi - dogru - yanlis;

                    sonuc.Detaylar.Add(new DenemeSonucDetay
                    {
                        DenemeSonuc = sonuc,
                        SinavTuruTestId = test.TestId,
                        Dogru = dogru,
                        Yanlis = yanlis,
                        Bos = bos
                    });
                }

                deneme.Sonuclar.Add(sonuc);
            }
        }
    }

    private void OdevleriOlustur(List<Ogrenci> ogrenciler)
    {
        var odevler = new List<Odev>();
        string[] konuBasliklari = ["Test Çözümü", "Konu Tekrarı", "Soru Bankası", "Deneme Analizi", "Eksik Konu Tamamlama"];

        foreach (var ogrenci in ogrenciler.Where(o => o.AktifMi))
        {
            var odevSayisi = _rastgele.Next(2, 4);

            for (var i = 0; i < odevSayisi; i++)
            {
                var dersId = DersHavuzu(ogrenci.Alan)[_rastgele.Next(DersHavuzu(ogrenci.Alan).Length)];
                var verilisTarihi = DateTime.Today.AddDays(-_rastgele.Next(3, 20));
                var sonTeslimTarihi = verilisTarihi.AddDays(_rastgele.Next(3, 10));
                var durumSecimi = _rastgele.Next(4);

                var odev = new Odev
                {
                    Ogrenci = ogrenci,
                    DersId = dersId,
                    KonuBasligi = konuBasliklari[_rastgele.Next(konuBasliklari.Length)],
                    ToplamSoruSayisi = _rastgele.Next(20, 60),
                    VerilisTarihi = verilisTarihi,
                    SonTeslimTarihi = sonTeslimTarihi
                };

                switch (durumSecimi)
                {
                    case 0:
                        odev.Durum = OdevDurumu.Tamamlandi;
                        odev.YapilmaTarihi = sonTeslimTarihi.AddDays(-1);
                        odev.TamamlananSoruSayisi = odev.ToplamSoruSayisi;
                        break;
                    case 1:
                        odev.Durum = OdevDurumu.EksikTamamlandi;
                        odev.YapilmaTarihi = sonTeslimTarihi;
                        odev.TamamlananSoruSayisi = odev.ToplamSoruSayisi / 2;
                        break;
                    default:
                        // yapılmamış bırakılıyor, tarihi geçmişse okuma anında otomatik "gecikti" görünecek
                        odev.Durum = OdevDurumu.Verildi;
                        break;
                }

                odevler.Add(odev);
            }
        }

        _baglam.AddRange(odevler);
    }

    private void GorusmeleriOlustur(List<Ogrenci> ogrenciler)
    {
        var gorusmeler = new List<Gorusme>();

        foreach (var ogrenci in ogrenciler.Where(o => o.AktifMi))
        {
            var gorusmeSayisi = _rastgele.Next(1, 3);

            for (var i = 0; i < gorusmeSayisi; i++)
            {
                gorusmeler.Add(new Gorusme
                {
                    Ogrenci = ogrenci,
                    Tarih = DateTime.Today.AddDays(-_rastgele.Next(1, 90)),
                    Tur = (GorusmeTuru)(_rastgele.Next(3) + 1),
                    Konu = GorusmeKonuPool[_rastgele.Next(GorusmeKonuPool.Length)],
                    Notlar = GorusmeNotPool[_rastgele.Next(GorusmeNotPool.Length)],
                    Sure = _rastgele.Next(10, 45)
                });
            }
        }

        _baglam.AddRange(gorusmeler);
    }

    private void HedefleriOlustur(List<Ogrenci> ogrenciler)
    {
        var hedefler = new List<Hedef>();

        foreach (var ogrenci in ogrenciler.Where(o => o.AktifMi))
        {
            var uygunlar = HedefPool.Where(h => h.Alan == ogrenci.Alan).ToList();
            if (uygunlar.Count == 0)
            {
                continue;
            }

            var secim = uygunlar[_rastgele.Next(uygunlar.Count)];
            var puanTuru = ogrenci.Alan switch
            {
                Alan.Sayisal => PuanTuru.Say,
                Alan.EsitAgirlik => PuanTuru.Ea,
                Alan.Sozel => PuanTuru.Soz,
                _ => PuanTuru.Dil
            };

            var hedef = new Hedef
            {
                Ogrenci = ogrenci,
                HedefUniversite = secim.Universite,
                HedefBolum = secim.Bolum,
                PuanTuru = puanTuru,
                HedefSiralama = _rastgele.Next(5000, 150000),
                HedefToplamNet = _rastgele.Next(60, 110),
                AktifMi = true
            };

            foreach (var dersId in DersHavuzu(ogrenci.Alan))
            {
                hedef.DersNetleri.Add(new HedefDersNeti { Hedef = hedef, DersId = dersId, HedefNet = _rastgele.Next(15, 38) });
            }

            hedefler.Add(hedef);
        }

        _baglam.AddRange(hedefler);
    }

    private void CalismaProgramlariniOlustur(List<Ogrenci> ogrenciler)
    {
        var programlar = new List<CalismaProgrami>();
        var gunler = new[] { Gun.Pazartesi, Gun.Sali, Gun.Carsamba, Gun.Persembe, Gun.Cuma, Gun.Cumartesi };
        var haftaBaslangici = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);

        foreach (var ogrenci in ogrenciler.Where(o => o.AktifMi))
        {
            // her öğrenciye değil, yaklaşık yarısına çalışma programı veriyoruz
            if (_rastgele.Next(2) == 0)
            {
                continue;
            }

            var program = new CalismaProgrami { Ogrenci = ogrenci, HaftaBaslangicTarihi = haftaBaslangici };
            var dersHavuzu = DersHavuzu(ogrenci.Alan);

            foreach (var gun in gunler.Take(_rastgele.Next(3, 6)))
            {
                program.Satirlar.Add(new CalismaProgramiSatiri
                {
                    CalismaProgrami = program,
                    Gun = gun,
                    BaslangicSaati = new TimeSpan(19, 0, 0),
                    BitisSaati = new TimeSpan(20, 30, 0),
                    DersId = dersHavuzu[_rastgele.Next(dersHavuzu.Length)],
                    Aciklama = "Konu tekrarı ve soru çözümü",
                    TamamlandiMi = _rastgele.Next(2) == 0
                });
            }

            programlar.Add(program);
        }

        _baglam.AddRange(programlar);
    }

    private void KonuTakipleriniOlustur(List<Ogrenci> ogrenciler)
    {
        var takipler = new List<OgrenciKonuTakip>();
        var konuHavuzu = KonuIdHavuzu();

        foreach (var ogrenci in ogrenciler.Where(o => o.AktifMi))
        {
            var konular = konuHavuzu(ogrenci.Alan);

            foreach (var konuId in konular)
            {
                takipler.Add(new OgrenciKonuTakip
                {
                    Ogrenci = ogrenci,
                    KonuId = konuId,
                    Durum = (KonuTakipDurumu)(_rastgele.Next(4) + 1)
                });
            }
        }

        _baglam.AddRange(takipler);
    }

    // dershaneninkinden farklı olarak burada dersin bütün konularını kullanıyoruz, migration'daki tohum konu kimlikleri sabit
    private static Func<Alan, int[]> KonuIdHavuzu() => alan => alan switch
    {
        Alan.Sayisal => [4, 5, 6, 13, 14, 15, 16, 17, 18, 19, 20, 21],
        Alan.EsitAgirlik => [4, 5, 6, 22, 23, 24, 25, 26, 27, 28, 29, 30],
        Alan.Sozel => [22, 23, 24, 25, 26, 27, 28, 29, 30, 37, 38, 39],
        Alan.Dil => [43, 44, 45, 1, 2, 3],
        _ => [1, 2, 3]
    };

    private void GorevleriOlustur(List<Ogrenci> ogrenciler)
    {
        var gorevler = new List<Gorev>();
        var aktifOgrenciler = ogrenciler.Where(o => o.AktifMi).ToList();

        // birkaçı öğrenciye bağlı değil, genel görevler
        for (var i = 0; i < 5; i++)
        {
            gorevler.Add(new Gorev
            {
                Baslik = GorevBaslikPool[_rastgele.Next(GorevBaslikPool.Length)],
                Tarih = DateTime.Today.AddDays(_rastgele.Next(-3, 5)),
                Oncelik = (GorevOnceligi)(_rastgele.Next(3) + 1),
                TamamlandiMi = _rastgele.Next(3) == 0
            });
        }

        // bir kısmı bugüne ait olsun ki ana sayfadaki "bugün görüşülecekler" listesi de dolu görünsün
        foreach (var ogrenci in aktifOgrenciler.OrderBy(_ => _rastgele.Next()).Take(5))
        {
            gorevler.Add(new Gorev
            {
                Baslik = $"{ogrenci.Ad} {ogrenci.Soyad} ile görüş",
                Aciklama = GorevBaslikPool[_rastgele.Next(GorevBaslikPool.Length)],
                Tarih = DateTime.Today,
                Ogrenci = ogrenci,
                Oncelik = (GorevOnceligi)(_rastgele.Next(3) + 1),
                TamamlandiMi = false
            });
        }

        foreach (var ogrenci in aktifOgrenciler.OrderBy(_ => _rastgele.Next()).Take(8))
        {
            gorevler.Add(new Gorev
            {
                Baslik = $"{ogrenci.Ad} {ogrenci.Soyad} takibi",
                Tarih = DateTime.Today.AddDays(_rastgele.Next(-10, 10)),
                Ogrenci = ogrenci,
                Oncelik = (GorevOnceligi)(_rastgele.Next(3) + 1),
                TamamlandiMi = _rastgele.Next(2) == 0
            });
        }

        _baglam.AddRange(gorevler);
    }

    private void NotlariOlustur()
    {
        var notlar = NotPool.Select(n => new Not { Baslik = n.Baslik, Icerik = n.Icerik }).ToList();
        _baglam.AddRange(notlar);
    }
}
