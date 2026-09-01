using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class CalismaProgramiEslemeleri
{
    public static CalismaProgramiSatiriDTO Dto(this CalismaProgramiSatiri satir) => new()
    {
        Id = satir.Id,
        Gun = satir.Gun,
        BaslangicSaati = satir.BaslangicSaati,
        BitisSaati = satir.BitisSaati,
        DersId = satir.DersId,
        DersAdi = satir.Ders?.Ad,
        Aciklama = satir.Aciklama,
        TamamlandiMi = satir.TamamlandiMi
    };

    public static CalismaProgramiSatiriKaydetDTO KaydetDto(this CalismaProgramiSatiri satir) => new()
    {
        Id = satir.Id,
        Gun = satir.Gun,
        BaslangicSaati = satir.BaslangicSaati,
        BitisSaati = satir.BitisSaati,
        DersId = satir.DersId,
        Aciklama = satir.Aciklama,
        TamamlandiMi = satir.TamamlandiMi
    };

    public static CalismaProgramiSatiri Entity(this CalismaProgramiSatiriKaydetDTO dto) => new()
    {
        Gun = dto.Gun,
        BaslangicSaati = dto.BaslangicSaati,
        BitisSaati = dto.BitisSaati,
        DersId = dto.DersId,
        Aciklama = dto.Aciklama,
        TamamlandiMi = dto.TamamlandiMi
    };

    public static void Uygula(this CalismaProgramiSatiriKaydetDTO dto, CalismaProgramiSatiri satir)
    {
        satir.Gun = dto.Gun;
        satir.BaslangicSaati = dto.BaslangicSaati;
        satir.BitisSaati = dto.BitisSaati;
        satir.DersId = dto.DersId;
        satir.Aciklama = dto.Aciklama;
        satir.TamamlandiMi = dto.TamamlandiMi;
    }

    public static CalismaProgramiDTO Dto(this CalismaProgrami calismaProgrami) => new()
    {
        Id = calismaProgrami.Id,
        OgrenciId = calismaProgrami.OgrenciId,
        HaftaBaslangicTarihi = calismaProgrami.HaftaBaslangicTarihi,
        Aciklama = calismaProgrami.Aciklama,
        Satirlar = calismaProgrami.Satirlar.Select(s => s.Dto()).ToList()
    };

    public static CalismaProgramiKaydetDTO KaydetDto(this CalismaProgrami calismaProgrami) => new()
    {
        Id = calismaProgrami.Id,
        OgrenciId = calismaProgrami.OgrenciId,
        HaftaBaslangicTarihi = calismaProgrami.HaftaBaslangicTarihi,
        Aciklama = calismaProgrami.Aciklama,
        Satirlar = calismaProgrami.Satirlar.Select(s => s.KaydetDto()).ToList()
    };

    public static CalismaProgrami Entity(this CalismaProgramiKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        HaftaBaslangicTarihi = dto.HaftaBaslangicTarihi,
        Aciklama = dto.Aciklama
    };

    public static void Uygula(this CalismaProgramiKaydetDTO dto, CalismaProgrami calismaProgrami)
    {
        calismaProgrami.HaftaBaslangicTarihi = dto.HaftaBaslangicTarihi;
        calismaProgrami.Aciklama = dto.Aciklama;
    }
}
