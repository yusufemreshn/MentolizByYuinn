using System.Text;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class VeliIletisimServisi : IVeliIletisimServisi
{
    private readonly IOgrenciServisi _ogrenciServisi;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IHedefServisi _hedefServisi;

    public VeliIletisimServisi(
        IOgrenciServisi ogrenciServisi,
        IDenemeAnaliziServisi denemeAnaliziServisi,
        IOdevServisi odevServisi,
        IHedefServisi hedefServisi)
    {
        _ogrenciServisi = ogrenciServisi;
        _denemeAnaliziServisi = denemeAnaliziServisi;
        _odevServisi = odevServisi;
        _hedefServisi = hedefServisi;
    }

    public async Task<string> MesajOlusturAsync(int ogrenciId)
    {
        var ogrenci = await _ogrenciServisi.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return string.Empty;
        }

        // öğrencinin katıldığı ilk sınav türü üzerinden analiz çekiliyor, veli raporu ekranı da aynı mantığı kullanıyor
        var katildigiSinavTurleri = await _denemeAnaliziServisi.OgrenciKatildigiSinavTurleriniListeleAsync(ogrenciId);
        var seciliSinavTuruId = katildigiSinavTurleri.FirstOrDefault()?.Id;
        var denemeAnalizi = seciliSinavTuruId.HasValue
            ? await _denemeAnaliziServisi.AnaliziHesaplaAsync(ogrenciId, seciliSinavTuruId.Value)
            : new OgrenciDenemeAnaliziDTO();

        var odevOzeti = await _odevServisi.OgrenciOzetiHesaplaAsync(ogrenciId);
        var hedefKarsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(ogrenciId);

        var metin = new StringBuilder();
        metin.AppendLine($"{VeliHitabiOlustur(ogrenci.Veliler)},");
        metin.AppendLine();
        metin.Append($"{ogrenci.AdSoyad} ile ilgili kısa bir bilgilendirme paylaşmak istedim. ");

        DenemeCumlesiEkle(metin, denemeAnalizi);
        metin.Append($"Bu dönem verilen ödevlerin %{odevOzeti.TamamlamaOrani}'ini tamamladı ({odevOzeti.TamamlananOdevSayisi}/{odevOzeti.ToplamOdevSayisi}). ");
        HedefCumlesiEkle(metin, hedefKarsilastirma?.ToplamGuncelNet, hedefKarsilastirma?.ToplamHedefNet);

        metin.AppendLine();
        metin.AppendLine();
        metin.Append("Sorularınız olursa benimle iletişime geçebilirsiniz. İyi çalışmalar dilerim.");

        return metin.ToString();
    }

    // veli listesinde birincil iletişim işaretli olan öne çıkıyor, hiçbiri işaretli değilse ilk veli kullanılıyor
    private static string VeliHitabiOlustur(List<VeliDTO> veliler)
    {
        var birincil = veliler.FirstOrDefault(v => v.BirincilIletisimMi) ?? veliler.FirstOrDefault();
        if (birincil is null)
        {
            return "Sayın Velimiz";
        }

        return birincil.Yakinlik switch
        {
            Yakinlik.Anne => $"Sayın {birincil.Ad} Hanım",
            Yakinlik.Baba => $"Sayın {birincil.Ad} Bey",
            _ => $"Sayın {birincil.Ad} {birincil.Soyad}"
        };
    }

    private static void DenemeCumlesiEkle(StringBuilder metin, OgrenciDenemeAnaliziDTO denemeAnalizi)
    {
        if (!denemeAnalizi.SonDenemeToplamNet.HasValue)
        {
            metin.Append("Henüz deneme sonucu girilmedi. ");
            return;
        }

        metin.Append($"En son katıldığı {denemeAnalizi.SonDenemeAdi} denemesinde toplam neti {denemeAnalizi.SonDenemeToplamNet.Value:0.##} oldu");

        var degisim = SonIkiDenemeDegisimiHesapla(denemeAnalizi.NetGelisimi);
        if (degisim is null)
        {
            metin.Append(". ");
        }
        else if (degisim >= 0)
        {
            metin.Append($", önceki denemeye göre {degisim.Value:0.##} net artış gösterdi. ");
        }
        else
        {
            metin.Append($", önceki denemeye göre {Math.Abs(degisim.Value):0.##} net düşüş yaşadı. ");
        }
    }

    private static void HedefCumlesiEkle(StringBuilder metin, decimal? guncelNet, decimal? hedefNet)
    {
        if (guncelNet is null || hedefNet is null)
        {
            return;
        }

        var fark = guncelNet.Value - hedefNet.Value;
        metin.Append(fark >= 0
            ? "Hedeflediği toplam neti şu an itibarıyla yakalamış durumda. "
            : $"Hedeflediği toplam nete göre şu an {Math.Abs(fark):0.##} net geride, birlikte bu farkı kapatmaya çalışıyoruz. ");
    }

    // net gelişimi zaten tarihe göre sıralı geliyor, burada son iki değeri karşılaştırıyoruz
    private static decimal? SonIkiDenemeDegisimiHesapla(List<OgrenciNetGelisimiDTO> netGelisimi)
    {
        if (netGelisimi.Count < 2)
        {
            return null;
        }

        var sonIki = netGelisimi.TakeLast(2).ToList();
        return sonIki[1].ToplamNet - sonIki[0].ToplamNet;
    }
}
