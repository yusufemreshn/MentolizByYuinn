using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.DataAccess.Seed;

// migration ile birlikte veritabanına giren sabit başlangıç verisi, dersler ve sınav türü tanımları burada
public static class BaslangicVerisi
{
    // HasData tarih alanı için elle set edilmiş sabit bir değer istiyor, çalışma zamanındaki gerçek tarihle karışmasın
    private static readonly DateTime TohumTarihi = new(2026, 1, 1);

    public static Ders[] Dersler() =>
    [
        DersOlustur(1, "Türkçe", "TR", DersKategori.Temel, 1),
        DersOlustur(2, "Matematik", "MAT", DersKategori.Temel, 2),
        DersOlustur(3, "Sosyal Bilimler", "SOS", DersKategori.Temel, 3),
        DersOlustur(4, "Fen Bilimleri", "FEN", DersKategori.Temel, 4),
        DersOlustur(5, "Fizik", "FIZ", DersKategori.Sayisal, 5),
        DersOlustur(6, "Kimya", "KIM", DersKategori.Sayisal, 6),
        DersOlustur(7, "Biyoloji", "BIY", DersKategori.Sayisal, 7),
        DersOlustur(8, "Türk Dili ve Edebiyatı", "TDE", DersKategori.Sozel, 8),
        DersOlustur(9, "Tarih", "TAR", DersKategori.Sozel, 9),
        DersOlustur(10, "Coğrafya", "COG", DersKategori.Sozel, 10),
        // ayt sözelde tarih ve coğrafyanın ikinci oturumu ayrı derstir, müfredatı farklı
        DersOlustur(11, "Tarih-2", "TAR2", DersKategori.Sozel, 11),
        DersOlustur(12, "Coğrafya-2", "COG2", DersKategori.Sozel, 12),
        DersOlustur(13, "Felsefe Grubu", "FEL", DersKategori.Sozel, 13),
        DersOlustur(14, "Din Kültürü ve Ahlak Bilgisi", "DIN", DersKategori.Sozel, 14),
        DersOlustur(15, "Yabancı Dil", "YD", DersKategori.YabanciDil, 15)
    ];

    public static SinavTuru[] SinavTurleri() =>
    [
        SinavTuruOlustur(1, "TYT", "TYT", 1),
        SinavTuruOlustur(2, "AYT Sayısal", "AYT_SAY", 2),
        SinavTuruOlustur(3, "AYT Eşit Ağırlık", "AYT_EA", 3),
        SinavTuruOlustur(4, "AYT Sözel", "AYT_SOZ", 4),
        SinavTuruOlustur(5, "YDT", "YDT", 5)
    ];

    public static SinavTuruTest[] SinavTuruTestleri() =>
    [
        // tyt, yüz yirmi soru
        SinavTuruTestOlustur(1, 1, 1, 40, 1),
        SinavTuruTestOlustur(2, 1, 2, 40, 2),
        SinavTuruTestOlustur(3, 1, 3, 20, 3),
        SinavTuruTestOlustur(4, 1, 4, 20, 4),

        // ayt sayısal, seksen soru
        SinavTuruTestOlustur(5, 2, 2, 40, 1),
        SinavTuruTestOlustur(6, 2, 5, 14, 2),
        SinavTuruTestOlustur(7, 2, 6, 13, 3),
        SinavTuruTestOlustur(8, 2, 7, 13, 4),

        // ayt eşit ağırlık, seksen soru
        SinavTuruTestOlustur(9, 3, 2, 40, 1),
        SinavTuruTestOlustur(10, 3, 8, 24, 2),
        SinavTuruTestOlustur(11, 3, 9, 10, 3),
        SinavTuruTestOlustur(12, 3, 10, 6, 4),

        // ayt sözel, seksen soru
        SinavTuruTestOlustur(13, 4, 8, 24, 1),
        SinavTuruTestOlustur(14, 4, 9, 10, 2),
        SinavTuruTestOlustur(15, 4, 10, 6, 3),
        SinavTuruTestOlustur(16, 4, 11, 11, 4),
        SinavTuruTestOlustur(17, 4, 12, 11, 5),
        SinavTuruTestOlustur(18, 4, 13, 12, 6),
        SinavTuruTestOlustur(19, 4, 14, 6, 7),

        // ydt, seksen soru, ösym'nin standart yabancı dil testi formatı
        SinavTuruTestOlustur(20, 5, 15, 80, 1)
    ];

    public static Konu[] Konular() =>
    [
        // türkçe
        KonuOlustur(1, 1, "Sözcükte Anlam", KonuSeviyesi.Kolay, 1),
        KonuOlustur(2, 1, "Cümlede Anlam", KonuSeviyesi.Orta, 2),
        KonuOlustur(3, 1, "Paragrafta Anlam", KonuSeviyesi.Zor, 3),

        // matematik
        KonuOlustur(4, 2, "Temel Kavramlar", KonuSeviyesi.Kolay, 1),
        KonuOlustur(5, 2, "Sayı Basamakları ve Bölünebilme", KonuSeviyesi.Orta, 2),
        KonuOlustur(6, 2, "Fonksiyonlar", KonuSeviyesi.Zor, 3),

        // sosyal bilimler
        KonuOlustur(7, 3, "İlk Uygarlıklar", KonuSeviyesi.Kolay, 1),
        KonuOlustur(8, 3, "Doğa ve İnsan", KonuSeviyesi.Orta, 2),
        KonuOlustur(9, 3, "Bilgi Felsefesi", KonuSeviyesi.Zor, 3),

        // fen bilimleri
        KonuOlustur(10, 4, "Madde ve Özellikleri", KonuSeviyesi.Kolay, 1),
        KonuOlustur(11, 4, "Hareket ve Kuvvet", KonuSeviyesi.Orta, 2),
        KonuOlustur(12, 4, "Hücre", KonuSeviyesi.Zor, 3),

        // fizik
        KonuOlustur(13, 5, "Fizik Bilimine Giriş", KonuSeviyesi.Kolay, 1),
        KonuOlustur(14, 5, "Elektrik ve Manyetizma", KonuSeviyesi.Orta, 2),
        KonuOlustur(15, 5, "Dalgalar", KonuSeviyesi.Zor, 3),

        // kimya
        KonuOlustur(16, 6, "Kimya Bilimi", KonuSeviyesi.Kolay, 1),
        KonuOlustur(17, 6, "Atom ve Periyodik Sistem", KonuSeviyesi.Orta, 2),
        KonuOlustur(18, 6, "Asit Baz Dengesi", KonuSeviyesi.Zor, 3),

        // biyoloji
        KonuOlustur(19, 7, "Canlıların Ortak Özellikleri", KonuSeviyesi.Kolay, 1),
        KonuOlustur(20, 7, "Hücre Bölünmeleri", KonuSeviyesi.Orta, 2),
        KonuOlustur(21, 7, "Kalıtım", KonuSeviyesi.Zor, 3),

        // türk dili ve edebiyatı
        KonuOlustur(22, 8, "Anlam Bilgisi", KonuSeviyesi.Kolay, 1),
        KonuOlustur(23, 8, "Edebiyat Akımları", KonuSeviyesi.Orta, 2),
        KonuOlustur(24, 8, "Divan Edebiyatı", KonuSeviyesi.Zor, 3),

        // tarih
        KonuOlustur(25, 9, "Tarih ve Zaman", KonuSeviyesi.Kolay, 1),
        KonuOlustur(26, 9, "İslamiyet Öncesi Türk Tarihi", KonuSeviyesi.Orta, 2),
        KonuOlustur(27, 9, "Türk İslam Devletleri", KonuSeviyesi.Zor, 3),

        // coğrafya
        KonuOlustur(28, 10, "Doğa ve İnsan", KonuSeviyesi.Kolay, 1),
        KonuOlustur(29, 10, "İklim Bilgisi", KonuSeviyesi.Orta, 2),
        KonuOlustur(30, 10, "Nüfus ve Yerleşme", KonuSeviyesi.Zor, 3),

        // tarih-2
        KonuOlustur(31, 11, "XIX. Yüzyılda Osmanlı Devleti", KonuSeviyesi.Kolay, 1),
        KonuOlustur(32, 11, "Milli Mücadele", KonuSeviyesi.Orta, 2),
        KonuOlustur(33, 11, "Atatürk İlkeleri ve İnkılapları", KonuSeviyesi.Zor, 3),

        // coğrafya-2
        KonuOlustur(34, 12, "Ekosistem", KonuSeviyesi.Kolay, 1),
        KonuOlustur(35, 12, "Türkiye Ekonomisi", KonuSeviyesi.Orta, 2),
        KonuOlustur(36, 12, "Çevre ve Toplum", KonuSeviyesi.Zor, 3),

        // felsefe grubu
        KonuOlustur(37, 13, "Felsefeye Giriş", KonuSeviyesi.Kolay, 1),
        KonuOlustur(38, 13, "Bilim Felsefesi", KonuSeviyesi.Orta, 2),
        KonuOlustur(39, 13, "Ahlak Felsefesi", KonuSeviyesi.Zor, 3),

        // din kültürü ve ahlak bilgisi
        KonuOlustur(40, 14, "Bilgi ve İnanç", KonuSeviyesi.Kolay, 1),
        KonuOlustur(41, 14, "İslam ve İbadet", KonuSeviyesi.Orta, 2),
        KonuOlustur(42, 14, "Gençlik ve Değerler", KonuSeviyesi.Zor, 3),

        // yabancı dil
        KonuOlustur(43, 15, "Kelime Bilgisi", KonuSeviyesi.Kolay, 1),
        KonuOlustur(44, 15, "Dil Bilgisi Yapıları", KonuSeviyesi.Orta, 2),
        KonuOlustur(45, 15, "Okuduğunu Anlama", KonuSeviyesi.Zor, 3)
    ];

    private static Ders DersOlustur(int id, string ad, string kisaAd, DersKategori kategori, int sira) => new()
    {
        Id = id,
        Ad = ad,
        KisaAd = kisaAd,
        Kategori = kategori,
        Sira = sira,
        AktifMi = true,
        OlusturmaTarihi = TohumTarihi,
        GuncellemeTarihi = TohumTarihi,
        SilindiMi = false
    };

    private static SinavTuru SinavTuruOlustur(int id, string ad, string kod, int sira) => new()
    {
        Id = id,
        Ad = ad,
        Kod = kod,
        Sira = sira,
        AktifMi = true,
        OlusturmaTarihi = TohumTarihi,
        GuncellemeTarihi = TohumTarihi,
        SilindiMi = false
    };

    private static SinavTuruTest SinavTuruTestOlustur(int id, int sinavTuruId, int dersId, int soruSayisi, int sira) => new()
    {
        Id = id,
        SinavTuruId = sinavTuruId,
        DersId = dersId,
        SoruSayisi = soruSayisi,
        Sira = sira,
        OlusturmaTarihi = TohumTarihi,
        GuncellemeTarihi = TohumTarihi,
        SilindiMi = false
    };

    private static Konu KonuOlustur(int id, int dersId, string ad, KonuSeviyesi seviye, int sira) => new()
    {
        Id = id,
        DersId = dersId,
        Ad = ad,
        Seviye = seviye,
        Sira = sira,
        AktifMi = true,
        OlusturmaTarihi = TohumTarihi,
        GuncellemeTarihi = TohumTarihi,
        SilindiMi = false
    };
}
