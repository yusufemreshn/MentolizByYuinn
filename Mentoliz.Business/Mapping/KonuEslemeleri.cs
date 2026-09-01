using Mentoliz.Business.Dto.Konu;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class KonuEslemeleri
{
    public static KonuDTO Dto(this Konu konu) => new()
    {
        Id = konu.Id,
        DersId = konu.DersId,
        DersAdi = konu.Ders?.Ad ?? string.Empty,
        Ad = konu.Ad,
        Seviye = konu.Seviye,
        Sira = konu.Sira,
        AktifMi = konu.AktifMi
    };

    public static KonuKaydetDTO KaydetDto(this Konu konu) => new()
    {
        Id = konu.Id,
        DersId = konu.DersId,
        Ad = konu.Ad,
        Seviye = konu.Seviye,
        Sira = konu.Sira,
        AktifMi = konu.AktifMi
    };

    public static Konu Entity(this KonuKaydetDTO dto) => new()
    {
        DersId = dto.DersId,
        Ad = dto.Ad,
        Seviye = dto.Seviye,
        Sira = dto.Sira,
        AktifMi = dto.AktifMi
    };

    public static void Uygula(this KonuKaydetDTO dto, Konu konu)
    {
        konu.DersId = dto.DersId;
        konu.Ad = dto.Ad;
        konu.Seviye = dto.Seviye;
        konu.Sira = dto.Sira;
        konu.AktifMi = dto.AktifMi;
    }

    public static OgrenciKonuTakipDTO Dto(this OgrenciKonuTakip takip) => new()
    {
        Id = takip.Id,
        OgrenciId = takip.OgrenciId,
        KonuId = takip.KonuId,
        KonuAdi = takip.Konu?.Ad ?? string.Empty,
        DersId = takip.Konu?.DersId ?? 0,
        DersAdi = takip.Konu?.Ders?.Ad ?? string.Empty,
        Durum = takip.Durum,
        GuncellemeTarihi = takip.GuncellemeTarihi,
        Notlar = takip.Notlar
    };

    public static OgrenciKonuTakip Entity(this OgrenciKonuTakipKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        KonuId = dto.KonuId,
        Durum = dto.Durum,
        Notlar = dto.Notlar
    };

    public static void Uygula(this OgrenciKonuTakipKaydetDTO dto, OgrenciKonuTakip takip)
    {
        takip.Durum = dto.Durum;
        takip.Notlar = dto.Notlar;
    }
}
