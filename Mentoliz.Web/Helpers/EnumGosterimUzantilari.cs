using Mentoliz.Entities.Enums;

namespace Mentoliz.Web.Helpers;

// enum değerlerinin ekranda görünecek türkçe karşılıkları burada, entity'lerde veri anotasyonu kullanmadığımız için ayrı bir yerde tutuyoruz
public static class EnumGosterimUzantilari
{
    public static string GosterimAdi(this Cinsiyet cinsiyet) => cinsiyet switch
    {
        Cinsiyet.Erkek => "Erkek",
        Cinsiyet.Kiz => "Kız",
        _ => cinsiyet.ToString()
    };

    public static string GosterimAdi(this Alan alan) => alan switch
    {
        Alan.Sayisal => "Sayısal",
        Alan.EsitAgirlik => "Eşit Ağırlık",
        Alan.Sozel => "Sözel",
        Alan.Dil => "Dil",
        _ => alan.ToString()
    };

    public static string GosterimAdi(this Yakinlik yakinlik) => yakinlik switch
    {
        Yakinlik.Anne => "Anne",
        Yakinlik.Baba => "Baba",
        Yakinlik.Vasi => "Vasi",
        Yakinlik.Diger => "Diğer",
        _ => yakinlik.ToString()
    };

    public static string GosterimAdi(this SinifSeviyesi seviye) => seviye switch
    {
        SinifSeviyesi.Dokuz => "9. Sınıf",
        SinifSeviyesi.On => "10. Sınıf",
        SinifSeviyesi.OnBir => "11. Sınıf",
        SinifSeviyesi.OnIki => "12. Sınıf",
        SinifSeviyesi.Mezun => "Mezun",
        _ => seviye.ToString()
    };

    public static string GosterimAdi(this ProgramIstisnaTuru tur) => tur switch
    {
        ProgramIstisnaTuru.OzelDers => "Özel Ders",
        ProgramIstisnaTuru.Etut => "Etüt",
        ProgramIstisnaTuru.Telafi => "Telafi",
        ProgramIstisnaTuru.Iptal => "İptal",
        _ => tur.ToString()
    };

    public static string GosterimAdi(this OdevDurumu durum) => durum switch
    {
        OdevDurumu.Verildi => "Verildi",
        OdevDurumu.Tamamlandi => "Tamamlandı",
        OdevDurumu.EksikTamamlandi => "Eksik Tamamlandı",
        OdevDurumu.Gecikti => "Gecikti",
        OdevDurumu.Yapilmadi => "Yapılmadı",
        _ => durum.ToString()
    };

    public static string GosterimAdi(this GorusmeTuru tur) => tur switch
    {
        GorusmeTuru.OgrenciGorusmesi => "Öğrenci Görüşmesi",
        GorusmeTuru.VeliGorusmesi => "Veli Görüşmesi",
        GorusmeTuru.OgretmenGorusmesi => "Öğretmen Görüşmesi",
        _ => tur.ToString()
    };

    public static string GosterimAdi(this PuanTuru puanTuru) => puanTuru switch
    {
        PuanTuru.Say => "Sayısal",
        PuanTuru.Ea => "Eşit Ağırlık",
        PuanTuru.Soz => "Sözel",
        PuanTuru.Dil => "Dil",
        _ => puanTuru.ToString()
    };

    public static string GosterimAdi(this KonuTakipDurumu durum) => durum switch
    {
        KonuTakipDurumu.Baslanmadi => "Başlanmadı",
        KonuTakipDurumu.DevamEdiyor => "Devam Ediyor",
        KonuTakipDurumu.Tamamlandi => "Tamamlandı",
        KonuTakipDurumu.TekrarGerekli => "Tekrar Gerekli",
        _ => durum.ToString()
    };

    public static string GosterimAdi(this KonuSeviyesi seviye) => seviye switch
    {
        KonuSeviyesi.Kolay => "Kolay",
        KonuSeviyesi.Orta => "Orta",
        KonuSeviyesi.Zor => "Zor",
        _ => seviye.ToString()
    };

    public static string GosterimAdi(this GorevOnceligi oncelik) => oncelik switch
    {
        GorevOnceligi.Dusuk => "Düşük",
        GorevOnceligi.Normal => "Normal",
        GorevOnceligi.Yuksek => "Yüksek",
        _ => oncelik.ToString()
    };

    public static string GosterimAdi(this DevamsizlikDurumu durum) => durum switch
    {
        DevamsizlikDurumu.Geldi => "Geldi",
        DevamsizlikDurumu.Gelmedi => "Gelmedi",
        DevamsizlikDurumu.Mazeretli => "Mazeretli",
        _ => durum.ToString()
    };

    public static string GosterimAdi(this Gun gun) => gun switch
    {
        Gun.Pazartesi => "Pazartesi",
        Gun.Sali => "Salı",
        Gun.Carsamba => "Çarşamba",
        Gun.Persembe => "Perşembe",
        Gun.Cuma => "Cuma",
        Gun.Cumartesi => "Cumartesi",
        Gun.Pazar => "Pazar",
        _ => gun.ToString()
    };
}
