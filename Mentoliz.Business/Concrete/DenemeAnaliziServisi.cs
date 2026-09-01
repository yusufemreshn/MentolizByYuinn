using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class DenemeAnaliziServisi : IDenemeAnaliziServisi
{
    // ana sayfadaki en çok yükselen ve en çok düşen öğrenci listelerinde en fazla bu kadar öğrenci gösteriliyor
    private const int EnCokDegisenOgrenciSayisi = 5;

    // öğrenci detayındaki zayıf alan listesinde en fazla bu kadar ders gösteriliyor
    private const int ZayifAlanSayisi = 2;

    // öğrenci detayındaki en çok yükselen/düşen ders listelerinde en fazla bu kadar ders gösteriliyor
    private const int EnCokDegisenDersSayisi = 3;

    private readonly IUnitOfWork _unitOfWork;

    public DenemeAnaliziServisi(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SinavTuruDTO>> OgrenciKatildigiSinavTurleriniListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<DenemeSonuc>();

        var sinavTuruIdleri = await depo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi)
            .Select(s => s.Deneme.SinavTuruId)
            .Distinct()
            .ToListAsync();

        var sinavTuruDepo = _unitOfWork.RepositoryGetir<SinavTuru>();
        var sinavTurleri = await sinavTuruDepo.Sorgu
            .Where(s => sinavTuruIdleri.Contains(s.Id))
            .Include(s => s.Testler).ThenInclude(t => t.Ders)
            .OrderBy(s => s.Sira)
            .ToListAsync();

        return sinavTurleri.Select(s => s.Dto()).ToList();
    }

    public async Task<OgrenciDenemeAnaliziDTO> AnaliziHesaplaAsync(int ogrenciId, int sinavTuruId)
    {
        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var ogrenci = await ogrenciDepo.TekGetirAsync(ogrenciId);

        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var ogrenciSonuclari = await sonucDepo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi && s.Deneme.SinavTuruId == sinavTuruId)
            .Include(s => s.Deneme)
            .Include(s => s.Detaylar).ThenInclude(d => d.SinavTuruTest).ThenInclude(t => t.Ders)
            .ToListAsync();

        var analiz = new OgrenciDenemeAnaliziDTO
        {
            NetGelisimi = ogrenciSonuclari
                .OrderBy(s => s.Deneme.Tarih)
                .Select(s => new OgrenciNetGelisimiDTO
                {
                    DenemeId = s.DenemeId,
                    DenemeAdi = s.Deneme.Ad,
                    Tarih = s.Deneme.Tarih,
                    ToplamNet = s.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis))
                })
                .ToList()
        };

        var sonSonuc = ogrenciSonuclari.OrderByDescending(s => s.Deneme.Tarih).FirstOrDefault();
        if (sonSonuc is null)
        {
            return analiz;
        }

        analiz.SonDenemeAdi = sonSonuc.Deneme.Ad;
        analiz.SonDenemeDersNetleri = sonSonuc.Detaylar
            .OrderBy(d => d.SinavTuruTest.Sira)
            .Select(d => new OgrenciDersNetiDTO
            {
                DersAdi = d.SinavTuruTest.Ders.Ad,
                Net = NetHesaplayici.Hesapla(d.Dogru, d.Yanlis),
                SoruSayisi = d.SinavTuruTest.SoruSayisi
            })
            .ToList();
        analiz.SonDenemeToplamNet = analiz.SonDenemeDersNetleri.Sum(d => d.Net);

        // zayıf alan, net'in soru sayısına oranı en düşük olan derslerden çıkarılıyor
        analiz.ZayifAlanlar = analiz.SonDenemeDersNetleri
            .Where(d => d.SoruSayisi > 0)
            .OrderBy(d => d.Net / d.SoruSayisi)
            .Take(ZayifAlanSayisi)
            .Select(d => d.DersAdi)
            .ToList();

        // sınıf ortalaması ve kurum sıralaması aynı denemedeki bütün sonuçlar üzerinden hesaplanıyor
        var denemeTumSonuclar = await sonucDepo.Sorgu
            .Where(s => s.DenemeId == sonSonuc.DenemeId && !s.KatilmadiMi)
            .Include(s => s.Ogrenci)
            .Include(s => s.Detaylar)
            .ToListAsync();

        var netliSonuclar = denemeTumSonuclar
            .Select(s => new
            {
                s.OgrenciId,
                SinifId = s.Ogrenci.SinifId,
                ToplamNet = s.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis))
            })
            .OrderByDescending(s => s.ToplamNet)
            .ToList();

        analiz.ToplamKatilimci = netliSonuclar.Count;
        analiz.KurumSiralamasi = netliSonuclar.FindIndex(s => s.OgrenciId == ogrenciId) + 1;

        if (ogrenci?.SinifId.HasValue == true)
        {
            var sinifArkadaslari = netliSonuclar.Where(s => s.SinifId == ogrenci.SinifId.Value).ToList();
            if (sinifArkadaslari.Count > 0)
            {
                analiz.SinifOrtalamasi = Math.Round(sinifArkadaslari.Average(s => s.ToplamNet), 2);
            }
        }

        // son iki deneme arasında ortak derslerin net değişimi
        var oncekiSonuc = ogrenciSonuclari
            .Where(s => s.Deneme.Tarih < sonSonuc.Deneme.Tarih)
            .OrderByDescending(s => s.Deneme.Tarih)
            .FirstOrDefault();

        if (oncekiSonuc is not null)
        {
            var degisimler = new List<DersDegisimDTO>();
            foreach (var sonrakiDetay in sonSonuc.Detaylar)
            {
                var oncekiDetay = oncekiSonuc.Detaylar.FirstOrDefault(d => d.SinavTuruTest.DersId == sonrakiDetay.SinavTuruTest.DersId);
                if (oncekiDetay is null)
                {
                    continue;
                }

                var oncekiNet = NetHesaplayici.Hesapla(oncekiDetay.Dogru, oncekiDetay.Yanlis);
                var sonrakiNet = NetHesaplayici.Hesapla(sonrakiDetay.Dogru, sonrakiDetay.Yanlis);

                degisimler.Add(new DersDegisimDTO
                {
                    DersAdi = sonrakiDetay.SinavTuruTest.Ders.Ad,
                    OncekiNet = oncekiNet,
                    SonrakiNet = sonrakiNet,
                    Degisim = sonrakiNet - oncekiNet
                });
            }

            analiz.EnCokYukselenler = degisimler.Where(d => d.Degisim > 0).OrderByDescending(d => d.Degisim).Take(EnCokDegisenDersSayisi).ToList();
            analiz.EnCokDusenler = degisimler.Where(d => d.Degisim < 0).OrderBy(d => d.Degisim).Take(EnCokDegisenDersSayisi).ToList();
        }

        return analiz;
    }

    public async Task<DenemeAnaliziSonucuDTO?> SinifBazliOrtalamaNetleriHesaplaAsync()
    {
        var sonDeneme = await SonOrtakDenemeyiGetirAsync(SinavTuruKodlari.Tyt);
        if (sonDeneme is null)
        {
            return null;
        }

        return await DenemeSinifOrtalamalariniHesaplaAsync(sonDeneme, alanFiltresi: null);
    }

    public async Task<List<AlanBazliDenemeAnaliziDTO>> AlanBazliOrtalamaNetleriHesaplaAsync()
    {
        // kurum genelindeki tek ortak sınav tyt olduğu için tyt denemesi alanlar arasında ortak, tek seferde çekiliyor
        var tytDenemesi = await SonOrtakDenemeyiGetirAsync(SinavTuruKodlari.Tyt);

        var sonuc = new List<AlanBazliDenemeAnaliziDTO>();
        foreach (Alan alan in Enum.GetValues<Alan>())
        {
            var alanSonucu = new AlanBazliDenemeAnaliziDTO { Alan = alan };

            if (tytDenemesi is not null)
            {
                alanSonucu.TytSonucu = await DenemeSinifOrtalamalariniHesaplaAsync(tytDenemesi, alan);
            }

            var alanSinaviDenemesi = await SonOrtakDenemeyiGetirAsync(SinavTuruKodlari.AlanSinaviKodu(alan));
            if (alanSinaviDenemesi is not null)
            {
                alanSonucu.AlanSinaviSonucu = await DenemeSinifOrtalamalariniHesaplaAsync(alanSinaviDenemesi, alan);
            }

            sonuc.Add(alanSonucu);
        }

        return sonuc;
    }

    // belirtilen denemedeki sonuçları sınıf adına göre gruplayıp ortalama net hesaplıyor, alan filtresi verilirse sadece o alandaki sınıflar dahil ediliyor
    private async Task<DenemeAnaliziSonucuDTO> DenemeSinifOrtalamalariniHesaplaAsync(Deneme deneme, Alan? alanFiltresi)
    {
        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var sonuclar = await sonucDepo.Sorgu
            .Where(s => s.DenemeId == deneme.Id && !s.KatilmadiMi)
            .Include(s => s.Ogrenci).ThenInclude(o => o.Sinif)
            .Include(s => s.Detaylar)
            .ToListAsync();

        var sinifOrtalamalari = sonuclar
            .Where(s => s.Ogrenci.Sinif is not null && (alanFiltresi is null || s.Ogrenci.Sinif!.Alan == alanFiltresi))
            .GroupBy(s => s.Ogrenci.Sinif!.Ad)
            .Select(g => new SinifOrtalamaNetDTO
            {
                SinifAdi = g.Key,
                OrtalamaNet = Math.Round(g.Average(s => s.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis))), 2)
            })
            .OrderByDescending(s => s.OrtalamaNet)
            .ToList();

        return new DenemeAnaliziSonucuDTO
        {
            SinavTuruAdi = deneme.SinavTuru.Ad,
            DenemeAdi = deneme.Ad,
            SinifOrtalamaNetleri = sinifOrtalamalari
        };
    }

    public async Task<OgrenciNetDegisimiOzetiDTO> SonDenemeNetDegisimOzetiHesaplaAsync()
    {
        var degisimler = await TumOgrencilerNetDegisimleriniHesaplaAsync();

        return new OgrenciNetDegisimiOzetiDTO
        {
            EnCokYukselenler = degisimler.Where(d => d.Degisim > 0).OrderByDescending(d => d.Degisim).Take(EnCokDegisenOgrenciSayisi).ToList(),
            EnCokDusenler = degisimler.Where(d => d.Degisim < 0).OrderBy(d => d.Degisim).Take(EnCokDegisenOgrenciSayisi).ToList()
        };
    }

    public async Task<List<OgrenciNetDegisimiDTO>> TumOgrencilerNetDegisimleriniHesaplaAsync()
    {
        var degisimler = new List<OgrenciNetDegisimiDTO>();

        var sonDeneme = await SonOrtakDenemeyiGetirAsync(SinavTuruKodlari.Tyt);
        if (sonDeneme is null)
        {
            return degisimler;
        }

        var denemeDepo = _unitOfWork.RepositoryGetir<Deneme>();
        var oncekiDeneme = await denemeDepo.Sorgu
            .Where(d => d.SinavTuruId == sonDeneme.SinavTuruId && d.Tarih < sonDeneme.Tarih && d.Sonuclar.Any(s => !s.KatilmadiMi))
            .OrderByDescending(d => d.Tarih)
            .FirstOrDefaultAsync();

        if (oncekiDeneme is null)
        {
            return degisimler;
        }

        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var sonSonuclar = await sonucDepo.Sorgu
            .Where(s => s.DenemeId == sonDeneme.Id && !s.KatilmadiMi)
            .Include(s => s.Ogrenci)
            .Include(s => s.Detaylar)
            .ToListAsync();

        var oncekiSonuclar = await sonucDepo.Sorgu
            .Where(s => s.DenemeId == oncekiDeneme.Id && !s.KatilmadiMi)
            .Include(s => s.Detaylar)
            .ToListAsync();

        foreach (var sonSonuc in sonSonuclar)
        {
            var oncekiSonuc = oncekiSonuclar.FirstOrDefault(s => s.OgrenciId == sonSonuc.OgrenciId);
            if (oncekiSonuc is null)
            {
                continue;
            }

            var sonrakiNet = sonSonuc.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis));
            var oncekiNet = oncekiSonuc.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis));

            degisimler.Add(new OgrenciNetDegisimiDTO
            {
                OgrenciId = sonSonuc.OgrenciId,
                OgrenciAdSoyad = $"{sonSonuc.Ogrenci.Ad} {sonSonuc.Ogrenci.Soyad}",
                OncekiNet = oncekiNet,
                SonrakiNet = sonrakiNet,
                Degisim = sonrakiNet - oncekiNet
            });
        }

        return degisimler;
    }

    // verilen sınav türü kodunda en az bir katılımlı sonucu olan en güncel denemeyi getiriyor
    // tyt için bu kurum genelindeki tek ortak sınav olduğundan sınıf karşılaştırmalarında hep tyt kullanılıyor, ayt/ydt ise kendi alanındaki sınıfları karşılaştırmak için
    private async Task<Deneme?> SonOrtakDenemeyiGetirAsync(string sinavTuruKodu)
    {
        var denemeDepo = _unitOfWork.RepositoryGetir<Deneme>();

        return await denemeDepo.Sorgu
            .Where(d => d.SinavTuru.Kod == sinavTuruKodu && d.Sonuclar.Any(s => !s.KatilmadiMi))
            .Include(d => d.SinavTuru)
            .OrderByDescending(d => d.Tarih)
            .FirstOrDefaultAsync();
    }
}
