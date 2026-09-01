using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Ayar;

namespace Mentoliz.Business.Abstract;

public interface IYedeklemeServisi
{
    // veritabanı dosyasının tarih damgalı bir kopyasını hedef klasöre alır
    Task<IslemSonucu<string>> YedekAlAsync(string hedefKlasor);

    Task<List<YedekDosyaDTO>> YedekleriListeleAsync(string klasor);

    // canlı veritabanı dosyasının üzerine seçilen yedeği kopyalar
    Task<IslemSonucu> YedektenGeriYukleAsync(string yedekDosyaYolu);
}
