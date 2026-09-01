using Mentoliz.Business.Dto.Ders;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class DersEslemeleri
{
    public static DersDTO Dto(this Ders ders) => new()
    {
        Id = ders.Id,
        Ad = ders.Ad,
        KisaAd = ders.KisaAd,
        Kategori = ders.Kategori,
        Sira = ders.Sira,
        AktifMi = ders.AktifMi
    };

    public static Ders Entity(this DersKaydetDTO dto) => new()
    {
        Ad = dto.Ad,
        KisaAd = dto.KisaAd,
        Kategori = dto.Kategori,
        Sira = dto.Sira,
        AktifMi = dto.AktifMi
    };

    public static void Uygula(this DersKaydetDTO dto, Ders ders)
    {
        ders.Ad = dto.Ad;
        ders.KisaAd = dto.KisaAd;
        ders.Kategori = dto.Kategori;
        ders.Sira = dto.Sira;
        ders.AktifMi = dto.AktifMi;
    }
}
