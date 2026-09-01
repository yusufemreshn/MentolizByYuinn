using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

// deneme netindeki düşüş, ödev aksaması, görüşme aralığı ve hedeften geri kalma tek bir risk puanında birleşiyor
public class RiskDegerlendirmeServisi : IRiskDegerlendirmeServisi
{
    // dört etkenin toplam puana katkı ağırlıkları, toplamları bir ediyor
    private const decimal NetTrendAgirligi = 0.35m;
    private const decimal OdevAgirligi = 0.25m;
    private const decimal GorusmeAgirligi = 0.20m;
    private const decimal HedefAgirligi = 0.20m;

    // bu puanın üzerindeki öğrenciler için otomatik görev açılıyor
    private const int YuksekRiskEsigi = 50;

    // net düşüşünün her bir neti puana böyle yansıyor, örneğin dört net düşüş kırk puan risk katıyor
    private const int NetDususPuanCarpani = 10;

    // görüşülmeyen her gün puana böyle yansıyor
    private const int GorusmeGunPuanCarpani = 2;

    // hedeften geride kalınan her net puana böyle yansıyor
    private const int HedefFarkiPuanCarpani = 5;

    // otomatik açılan görevleri sonradan tanıyabilmek ve tekrar oluşturmamak için başlıkları bu önekle başlıyor
    private const string RiskGorevBasligiOnEki = "Risk uyarısı: ";

    // otomatik görev bugüne değil birkaç gün sonrasına açılıyor, "bugün görüşülecekler" listesini basmasın diye
    private const int RiskGoreviIleriGunSayisi = 3;

    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IHedefServisi _hedefServisi;
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;
    private readonly IGorevServisi _gorevServisi;

    public RiskDegerlendirmeServisi(
        IOgrenciServisi ogrenciServisi,
        IOdevServisi odevServisi,
        IHedefServisi hedefServisi,
        IGorusmeServisi gorusmeServisi,
        IDenemeAnaliziServisi denemeAnaliziServisi,
        IGorevServisi gorevServisi)
    {
        _ogrenciServisi = ogrenciServisi;
        _odevServisi = odevServisi;
        _hedefServisi = hedefServisi;
        _gorusmeServisi = gorusmeServisi;
        _denemeAnaliziServisi = denemeAnaliziServisi;
        _gorevServisi = gorevServisi;
    }

    public async Task<List<OgrenciRiskDTO>> DegerlendirAsync()
    {
        var aktifOgrenciler = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { AktifMi = true });

        // dört etkenin hepsi kurum genelinde tek seferde çekiliyor, öğrenci başına ayrı ayrı sorgu atmıyoruz
        var netDegisimleri = await _denemeAnaliziServisi.TumOgrencilerNetDegisimleriniHesaplaAsync();
        var uzunSureGorusulmeyenler = await _gorusmeServisi.UzunSureGorusulmeyenleriListeleAsync();

        var sonuc = new List<OgrenciRiskDTO>();
        foreach (var ogrenci in aktifOgrenciler)
        {
            var netDegisim = netDegisimleri.FirstOrDefault(d => d.OgrenciId == ogrenci.Id);
            var gorusmeBilgisi = uzunSureGorusulmeyenler.FirstOrDefault(g => g.OgrenciId == ogrenci.Id);
            var odevOzeti = await _odevServisi.OgrenciOzetiHesaplaAsync(ogrenci.Id);
            var hedefKarsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(ogrenci.Id);

            var netTrendPuani = NetTrendPuaniHesapla(netDegisim);
            var odevPuani = OdevPuaniHesapla(odevOzeti);
            var gorusmePuani = GorusmePuaniHesapla(gorusmeBilgisi);
            var hedefPuani = HedefPuaniHesapla(hedefKarsilastirma);

            var toplamPuan = (int)Math.Round(
                (netTrendPuani * NetTrendAgirligi) +
                (odevPuani * OdevAgirligi) +
                (gorusmePuani * GorusmeAgirligi) +
                (hedefPuani * HedefAgirligi));

            sonuc.Add(new OgrenciRiskDTO
            {
                OgrenciId = ogrenci.Id,
                OgrenciAdSoyad = $"{ogrenci.Ad} {ogrenci.Soyad}",
                RiskPuani = toplamPuan,
                NetTrendPuani = netTrendPuani,
                OdevPuani = odevPuani,
                GorusmePuani = gorusmePuani,
                HedefPuani = hedefPuani
            });
        }

        return sonuc.OrderByDescending(s => s.RiskPuani).ToList();
    }

    // net düştüyse risk artıyor, karşılaştırılacak iki deneme yoksa nötr kabul ediyoruz çünkü veri yokluğunu risk saymak yanıltıcı olur
    private static int NetTrendPuaniHesapla(OgrenciNetDegisimiDTO? netDegisim)
    {
        if (netDegisim is null || netDegisim.Degisim >= 0)
        {
            return 0;
        }

        return Math.Min(100, (int)(Math.Abs(netDegisim.Degisim) * NetDususPuanCarpani));
    }

    // hiç ödev verilmemiş öğrenciyi elinde olmayan bir sebeple cezalandırmamak için toplam ödev sayısı sıfırsa nötr sayıyoruz
    private static int OdevPuaniHesapla(OgrenciOdevOzetiDTO odevOzeti)
    {
        return odevOzeti.ToplamOdevSayisi == 0 ? 0 : 100 - odevOzeti.TamamlamaOrani;
    }

    // listede yoksa öğrenci yakın zamanda görüşülmüş demektir, hiç görüşülmemişse en yüksek puanı veriyoruz
    private static int GorusmePuaniHesapla(UzunSureGorusulmeyenOgrenciDTO? gorusmeBilgisi)
    {
        if (gorusmeBilgisi is null)
        {
            return 0;
        }

        return gorusmeBilgisi.GecenGunSayisi is null
            ? 100
            : Math.Min(100, gorusmeBilgisi.GecenGunSayisi.Value * GorusmeGunPuanCarpani);
    }

    // aktif hedef veya deneme sonucu yoksa karşılaştıracak bir şey yok, nötr sayıyoruz
    private static int HedefPuaniHesapla(HedefKarsilastirmaDTO? hedefKarsilastirma)
    {
        if (hedefKarsilastirma?.ToplamGuncelNet is null)
        {
            return 0;
        }

        var fark = hedefKarsilastirma.ToplamGuncelNet.Value - hedefKarsilastirma.ToplamHedefNet;
        return fark >= 0 ? 0 : Math.Min(100, (int)(Math.Abs(fark) * HedefFarkiPuanCarpani));
    }

    public async Task OtomatikGorevOlusturAsync(List<OgrenciRiskDTO> riskListesi)
    {
        var acikGorevler = await _gorevServisi.ListeleAsync(new GorevFiltreDTO { TamamlandiMi = false });

        foreach (var risk in riskListesi.Where(r => r.RiskPuani >= YuksekRiskEsigi))
        {
            var zatenAcikGoreviVar = acikGorevler.Any(g => g.OgrenciId == risk.OgrenciId && g.Baslik.StartsWith(RiskGorevBasligiOnEki, StringComparison.Ordinal));
            if (zatenAcikGoreviVar)
            {
                continue;
            }

            await _gorevServisi.EkleAsync(new GorevKaydetDTO
            {
                Baslik = $"{RiskGorevBasligiOnEki}{risk.OgrenciAdSoyad} ile görüş",
                // bugüne değil birkaç gün sonrasına atıyoruz, yoksa aynı anda çok sayıda öğrenci eşiği geçince ana sayfadaki "bugün görüşülecekler" listesi tamamen risk uyarılarına boğuluyor
                Tarih = DateTime.Today.AddDays(RiskGoreviIleriGunSayisi),
                OgrenciId = risk.OgrenciId,
                Oncelik = GorevOnceligi.Yuksek,
                TamamlandiMi = false
            });
        }
    }
}
