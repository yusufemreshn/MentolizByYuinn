namespace Mentoliz.Business.Helpers;

// haftalık hesaplamalarda hafta başlangıcı ve bitişi tek yerden bulunsun diye ayrı bir yardımcı sınıfa aldık
public static class HaftaYardimcisi
{
    private const int HaftaGunSayisi = 7;

    // pazartesiyi haftanın ilk günü sayıyoruz
    public static DateTime HaftaBaslangici(DateTime tarih)
    {
        var pazartesiyeUzaklik = ((int)tarih.DayOfWeek + 6) % 7;
        return tarih.Date.AddDays(-pazartesiyeUzaklik);
    }

    // bir sonraki pazartesiyi veriyor, aralık sorgularında "bu haftanın dışı" sınırı olarak kullanılıyor
    public static DateTime HaftaBitisi(DateTime haftaBaslangici) => haftaBaslangici.AddDays(HaftaGunSayisi);
}
