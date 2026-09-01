using Mentoliz.Business.Dto.Tercih;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class TercihEslemeleri
{
    public static OgrenciTercihiDTO Dto(this OgrenciTercihi tercih) => new()
    {
        Id = tercih.Id,
        OgrenciId = tercih.OgrenciId,
        Sira = tercih.Sira,
        UniversiteAdi = tercih.UniversiteAdi,
        BolumAdi = tercih.BolumAdi,
        TabanPuan = tercih.TabanPuan,
        Notlar = tercih.Notlar
    };

    public static OgrenciTercihiKaydetDTO KaydetDto(this OgrenciTercihi tercih) => new()
    {
        Id = tercih.Id,
        OgrenciId = tercih.OgrenciId,
        Sira = tercih.Sira,
        UniversiteAdi = tercih.UniversiteAdi,
        BolumAdi = tercih.BolumAdi,
        TabanPuan = tercih.TabanPuan,
        Notlar = tercih.Notlar
    };

    public static OgrenciTercihi Entity(this OgrenciTercihiKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        Sira = dto.Sira,
        UniversiteAdi = dto.UniversiteAdi,
        BolumAdi = dto.BolumAdi,
        TabanPuan = dto.TabanPuan,
        Notlar = dto.Notlar
    };

    public static void Uygula(this OgrenciTercihiKaydetDTO dto, OgrenciTercihi tercih)
    {
        tercih.Sira = dto.Sira;
        tercih.UniversiteAdi = dto.UniversiteAdi;
        tercih.BolumAdi = dto.BolumAdi;
        tercih.TabanPuan = dto.TabanPuan;
        tercih.Notlar = dto.Notlar;
    }
}
