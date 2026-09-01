using Mentoliz.Business.Dto.Sinif;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class SinifEslemeleri
{
    public static SinifDTO Dto(this Sinif sinif) => new()
    {
        Id = sinif.Id,
        Ad = sinif.Ad,
        Seviye = sinif.Seviye,
        Alan = sinif.Alan,
        Kontenjan = sinif.Kontenjan,
        AktifMi = sinif.AktifMi,
        OgrenciSayisi = sinif.Ogrenciler.Count
    };

    public static SinifKaydetDTO KaydetDto(this Sinif sinif) => new()
    {
        Id = sinif.Id,
        Ad = sinif.Ad,
        Seviye = sinif.Seviye,
        Alan = sinif.Alan,
        Kontenjan = sinif.Kontenjan,
        AktifMi = sinif.AktifMi
    };

    public static Sinif Entity(this SinifKaydetDTO dto) => new()
    {
        Ad = dto.Ad,
        Seviye = dto.Seviye,
        Alan = dto.Alan,
        Kontenjan = dto.Kontenjan,
        AktifMi = dto.AktifMi
    };

    public static void Uygula(this SinifKaydetDTO dto, Sinif sinif)
    {
        sinif.Ad = dto.Ad;
        sinif.Seviye = dto.Seviye;
        sinif.Alan = dto.Alan;
        sinif.Kontenjan = dto.Kontenjan;
        sinif.AktifMi = dto.AktifMi;
    }
}
