using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Helpers;

// sınav türü kodları migration ile sabit geldiği için burada elle referans veriyoruz, id yerine kod kullanmak taşınmaya karşı daha dayanıklı
public static class SinavTuruKodlari
{
    // alanı ne olursa olsun bütün öğrencilerin girdiği tek ortak sınav, sınıflar arası kıyaslamalar bu yüzden hep tyt üzerinden yapılıyor
    public const string Tyt = "TYT";

    public const string AytSayisal = "AYT_SAY";

    public const string AytEsitAgirlik = "AYT_EA";

    public const string AytSozel = "AYT_SOZ";

    public const string Ydt = "YDT";

    // her alanın kendi alan sınavı farklı kodda, dil alanının alan sınavı ayt değil ydt olduğu için ayrı bir eşleme gerekiyor
    public static string AlanSinaviKodu(Alan alan) => alan switch
    {
        Alan.Sayisal => AytSayisal,
        Alan.EsitAgirlik => AytEsitAgirlik,
        Alan.Sozel => AytSozel,
        Alan.Dil => Ydt,
        _ => throw new ArgumentOutOfRangeException(nameof(alan), alan, "Tanımsız alan değeri.")
    };
}
