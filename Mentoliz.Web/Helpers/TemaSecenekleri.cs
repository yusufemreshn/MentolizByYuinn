namespace Mentoliz.Web.Helpers;

// tek bir tema seçeneğini temsil ediyor, anahtar css tarafındaki data-theme değeriyle birebir aynı olmak zorunda
public record TemaSecenegi(string Anahtar, string GorunenAd);

// ayarlar sayfasındaki tema seçici ile temalar.css içindeki data-theme listesi burada tek elden eşleşiyor
public static class TemaSecenekleri
{
    // ayar hiç kaydedilmemişse veya bozuksa bu temaya dönülüyor
    public const string Varsayilan = "lacivert";

    public static readonly IReadOnlyList<TemaSecenegi> Tumu =
    [
        new("lacivert", "Lacivert Sabah"),
        new("zumrut", "Zümrüt Rehber"),
        new("minimal", "Minimal Kâğıt"),
        new("indigo", "Indigo Gece"),
        new("toprak", "Toprak Sıcaklığı")
    ];

    // kaydedilecek ya da html'e basılacak tema anahtarı listede yoksa kabul etmiyoruz, serbest metin buraya giremez
    public static bool GecerliMi(string? anahtar) => Tumu.Any(t => t.Anahtar == anahtar);
}
