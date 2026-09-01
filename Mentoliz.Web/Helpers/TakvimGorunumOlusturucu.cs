namespace Mentoliz.Web.Helpers;

// ay görünümü ızgarasının hangi günlerden oluşacağını hesaplıyor, pazartesi haftanın ilk günü sayılıyor
public static class TakvimGorunumOlusturucu
{
    private const int HaftaninGunSayisi = 7;

    // ay başının bulunduğu haftanın pazartesiyle, ay sonunun bulunduğu haftanın pazarı arasındaki bütün günleri döner
    public static List<DateTime> GunleriOlustur(DateTime ayBaslangici)
    {
        var ayBitisi = ayBaslangici.AddMonths(1).AddDays(-1);

        var izgaraBaslangici = ayBaslangici.AddDays(-PazartesiyeUzaklik(ayBaslangici));
        var izgaraBitisi = ayBitisi.AddDays(HaftaninGunSayisi - 1 - PazartesiyeUzaklik(ayBitisi));

        var gunler = new List<DateTime>();
        for (var gun = izgaraBaslangici; gun <= izgaraBitisi; gun = gun.AddDays(1))
        {
            gunler.Add(gun);
        }

        return gunler;
    }

    // .net'te pazar haftanın ilk günü sayılır, biz pazartesiden başlattığımız için farkı kendimiz hesaplıyoruz
    private static int PazartesiyeUzaklik(DateTime tarih) => ((int)tarih.DayOfWeek + 6) % 7;
}
