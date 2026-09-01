using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class DenemeEslemeleri
{
    public static SinavTuruTestDTO Dto(this SinavTuruTest test) => new()
    {
        Id = test.Id,
        SinavTuruId = test.SinavTuruId,
        DersId = test.DersId,
        DersAdi = test.Ders?.Ad ?? string.Empty,
        SoruSayisi = test.SoruSayisi,
        Sira = test.Sira
    };

    public static SinavTuruDTO Dto(this SinavTuru sinavTuru) => new()
    {
        Id = sinavTuru.Id,
        Ad = sinavTuru.Ad,
        Kod = sinavTuru.Kod,
        Sira = sinavTuru.Sira,
        AktifMi = sinavTuru.AktifMi,
        Testler = sinavTuru.Testler.OrderBy(t => t.Sira).Select(t => t.Dto()).ToList()
    };

    public static DenemeDTO Dto(this Deneme deneme) => new()
    {
        Id = deneme.Id,
        Ad = deneme.Ad,
        Tarih = deneme.Tarih,
        SinavTuruId = deneme.SinavTuruId,
        SinavTuruAdi = deneme.SinavTuru?.Ad ?? string.Empty,
        YayinAdi = deneme.YayinAdi,
        Aciklama = deneme.Aciklama,
        KatilimciSayisi = deneme.Sonuclar.Count(s => !s.KatilmadiMi)
    };

    public static DenemeKaydetDTO KaydetDto(this Deneme deneme) => new()
    {
        Id = deneme.Id,
        Ad = deneme.Ad,
        Tarih = deneme.Tarih,
        SinavTuruId = deneme.SinavTuruId,
        YayinAdi = deneme.YayinAdi,
        Aciklama = deneme.Aciklama
    };

    public static Deneme Entity(this DenemeKaydetDTO dto) => new()
    {
        Ad = dto.Ad,
        Tarih = dto.Tarih,
        SinavTuruId = dto.SinavTuruId,
        YayinAdi = dto.YayinAdi,
        Aciklama = dto.Aciklama
    };

    public static void Uygula(this DenemeKaydetDTO dto, Deneme deneme)
    {
        deneme.Ad = dto.Ad;
        deneme.Tarih = dto.Tarih;
        deneme.SinavTuruId = dto.SinavTuruId;
        deneme.YayinAdi = dto.YayinAdi;
        deneme.Aciklama = dto.Aciklama;
    }

    public static DenemeSonucDetayDTO Dto(this DenemeSonucDetay detay) => new()
    {
        Id = detay.Id,
        SinavTuruTestId = detay.SinavTuruTestId,
        DersAdi = detay.SinavTuruTest?.Ders?.Ad ?? string.Empty,
        SoruSayisi = detay.SinavTuruTest?.SoruSayisi ?? 0,
        Dogru = detay.Dogru,
        Yanlis = detay.Yanlis,
        Bos = detay.Bos,
        Net = NetHesaplayici.Hesapla(detay.Dogru, detay.Yanlis)
    };

    public static DenemeSonucDetayKaydetDTO KaydetDto(this DenemeSonucDetay detay) => new()
    {
        Id = detay.Id,
        SinavTuruTestId = detay.SinavTuruTestId,
        Dogru = detay.Dogru,
        Yanlis = detay.Yanlis,
        Bos = detay.Bos
    };

    public static DenemeSonucDetay Entity(this DenemeSonucDetayKaydetDTO dto) => new()
    {
        SinavTuruTestId = dto.SinavTuruTestId,
        Dogru = dto.Dogru,
        Yanlis = dto.Yanlis,
        Bos = dto.Bos
    };

    public static void Uygula(this DenemeSonucDetayKaydetDTO dto, DenemeSonucDetay detay)
    {
        detay.SinavTuruTestId = dto.SinavTuruTestId;
        detay.Dogru = dto.Dogru;
        detay.Yanlis = dto.Yanlis;
        detay.Bos = dto.Bos;
    }

    public static DenemeSonucDTO Dto(this DenemeSonuc sonuc)
    {
        var detaylar = sonuc.Detaylar.Select(d => d.Dto()).ToList();

        return new DenemeSonucDTO
        {
            Id = sonuc.Id,
            DenemeId = sonuc.DenemeId,
            OgrenciId = sonuc.OgrenciId,
            OgrenciAdSoyad = sonuc.Ogrenci is null ? string.Empty : $"{sonuc.Ogrenci.Ad} {sonuc.Ogrenci.Soyad}",
            Notlar = sonuc.Notlar,
            KatilmadiMi = sonuc.KatilmadiMi,
            Detaylar = detaylar,
            ToplamNet = detaylar.Sum(d => d.Net)
        };
    }

    public static DenemeSonucKaydetDTO KaydetDto(this DenemeSonuc sonuc) => new()
    {
        Id = sonuc.Id,
        DenemeId = sonuc.DenemeId,
        OgrenciId = sonuc.OgrenciId,
        Notlar = sonuc.Notlar,
        KatilmadiMi = sonuc.KatilmadiMi,
        Detaylar = sonuc.Detaylar.Select(d => d.KaydetDto()).ToList()
    };

    public static DenemeSonuc Entity(this DenemeSonucKaydetDTO dto) => new()
    {
        DenemeId = dto.DenemeId,
        OgrenciId = dto.OgrenciId,
        Notlar = dto.Notlar,
        KatilmadiMi = dto.KatilmadiMi
    };

    public static void Uygula(this DenemeSonucKaydetDTO dto, DenemeSonuc sonuc)
    {
        sonuc.Notlar = dto.Notlar;
        sonuc.KatilmadiMi = dto.KatilmadiMi;
    }
}
