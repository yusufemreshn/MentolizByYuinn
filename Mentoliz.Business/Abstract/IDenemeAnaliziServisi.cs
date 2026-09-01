using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Business.Abstract;

public interface IDenemeAnaliziServisi
{
    // öğrencinin sonucu bulunan sınav türlerini döner, analiz ekranındaki sekmeleri doldurmak için
    Task<List<SinavTuruDTO>> OgrenciKatildigiSinavTurleriniListeleAsync(int ogrenciId);

    Task<OgrenciDenemeAnaliziDTO> AnaliziHesaplaAsync(int ogrenciId, int sinavTuruId);

    // ana sayfadaki sınıf karşılaştırma grafiği için, en son giren tyt denemesindeki sınıf bazlı ortalama netler, hiç deneme yoksa null döner
    Task<DenemeAnaliziSonucuDTO?> SinifBazliOrtalamaNetleriHesaplaAsync();

    // deneme analiz sayfasındaki sekiz grafik için, her alanın hem tyt hem kendi alan sınavındaki (ayt veya ydt) sınıf ortalamaları
    Task<List<AlanBazliDenemeAnaliziDTO>> AlanBazliOrtalamaNetleriHesaplaAsync();

    // ana sayfada gösterilecek, en son deneme ile ondan önceki aynı türdeki deneme arasında en çok yükselen ve düşen öğrenciler
    Task<OgrenciNetDegisimiOzetiDTO> SonDenemeNetDegisimOzetiHesaplaAsync();

    // son iki tyt denemesi arasında karşılaştırılabilir sonucu olan bütün öğrencilerin net değişimi, risk puanlaması bunu kullanıyor
    Task<List<OgrenciNetDegisimiDTO>> TumOgrencilerNetDegisimleriniHesaplaAsync();
}
