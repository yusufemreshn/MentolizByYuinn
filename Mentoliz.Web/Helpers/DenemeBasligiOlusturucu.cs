namespace Mentoliz.Web.Helpers;

// deneme adı genelde sınav türünün adını zaten içeriyor, örneğin tyt genel deneme yirmi dört gibi, aynı kelimeyi başlıkta iki kere göstermemek için burada birleştiriliyor
public static class DenemeBasligiOlusturucu
{
    public static string Olustur(string sinavTuruAdi, string denemeAdi)
    {
        return denemeAdi.StartsWith(sinavTuruAdi, StringComparison.OrdinalIgnoreCase)
            ? denemeAdi
            : $"{sinavTuruAdi} ({denemeAdi})";
    }
}
