using Mentoliz.Business.Dto.Program;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class ProgramEslemeleri
{
    public static DersProgramiSatiriDTO Dto(this DersProgramiSatiri satir) => new()
    {
        Id = satir.Id,
        SinifId = satir.SinifId,
        Gun = satir.Gun,
        BaslangicSaati = satir.BaslangicSaati,
        BitisSaati = satir.BitisSaati,
        DersId = satir.DersId,
        DersAdi = satir.Ders?.Ad ?? string.Empty,
        DerslikAdi = satir.DerslikAdi,
        OgretmenAdi = satir.OgretmenAdi
    };

    public static DersProgramiSatiriKaydetDTO KaydetDto(this DersProgramiSatiri satir) => new()
    {
        Id = satir.Id,
        SinifId = satir.SinifId,
        Gun = satir.Gun,
        BaslangicSaati = satir.BaslangicSaati,
        BitisSaati = satir.BitisSaati,
        DersId = satir.DersId,
        DerslikAdi = satir.DerslikAdi,
        OgretmenAdi = satir.OgretmenAdi
    };

    public static DersProgramiSatiri Entity(this DersProgramiSatiriKaydetDTO dto) => new()
    {
        SinifId = dto.SinifId,
        Gun = dto.Gun,
        BaslangicSaati = dto.BaslangicSaati,
        BitisSaati = dto.BitisSaati,
        DersId = dto.DersId,
        DerslikAdi = dto.DerslikAdi,
        OgretmenAdi = dto.OgretmenAdi
    };

    public static void Uygula(this DersProgramiSatiriKaydetDTO dto, DersProgramiSatiri satir)
    {
        satir.SinifId = dto.SinifId;
        satir.Gun = dto.Gun;
        satir.BaslangicSaati = dto.BaslangicSaati;
        satir.BitisSaati = dto.BitisSaati;
        satir.DersId = dto.DersId;
        satir.DerslikAdi = dto.DerslikAdi;
        satir.OgretmenAdi = dto.OgretmenAdi;
    }

    public static OgrenciProgramIstisnasiDTO Dto(this OgrenciProgramIstisnasi istisna) => new()
    {
        Id = istisna.Id,
        OgrenciId = istisna.OgrenciId,
        Gun = istisna.Gun,
        BaslangicSaati = istisna.BaslangicSaati,
        BitisSaati = istisna.BitisSaati,
        DersId = istisna.DersId,
        DersAdi = istisna.Ders?.Ad,
        Tur = istisna.Tur,
        OgretmenAdi = istisna.OgretmenAdi,
        Aciklama = istisna.Aciklama,
        BaslangicTarihi = istisna.BaslangicTarihi,
        BitisTarihi = istisna.BitisTarihi
    };

    public static OgrenciProgramIstisnasiKaydetDTO KaydetDto(this OgrenciProgramIstisnasi istisna) => new()
    {
        Id = istisna.Id,
        OgrenciId = istisna.OgrenciId,
        Gun = istisna.Gun,
        BaslangicSaati = istisna.BaslangicSaati,
        BitisSaati = istisna.BitisSaati,
        DersId = istisna.DersId,
        Tur = istisna.Tur,
        OgretmenAdi = istisna.OgretmenAdi,
        Aciklama = istisna.Aciklama,
        BaslangicTarihi = istisna.BaslangicTarihi,
        BitisTarihi = istisna.BitisTarihi
    };

    public static OgrenciProgramIstisnasi Entity(this OgrenciProgramIstisnasiKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        Gun = dto.Gun,
        BaslangicSaati = dto.BaslangicSaati,
        BitisSaati = dto.BitisSaati,
        DersId = dto.DersId,
        Tur = dto.Tur,
        OgretmenAdi = dto.OgretmenAdi,
        Aciklama = dto.Aciklama,
        BaslangicTarihi = dto.BaslangicTarihi,
        BitisTarihi = dto.BitisTarihi
    };

    public static void Uygula(this OgrenciProgramIstisnasiKaydetDTO dto, OgrenciProgramIstisnasi istisna)
    {
        istisna.Gun = dto.Gun;
        istisna.BaslangicSaati = dto.BaslangicSaati;
        istisna.BitisSaati = dto.BitisSaati;
        istisna.DersId = dto.DersId;
        istisna.Tur = dto.Tur;
        istisna.OgretmenAdi = dto.OgretmenAdi;
        istisna.Aciklama = dto.Aciklama;
        istisna.BaslangicTarihi = dto.BaslangicTarihi;
        istisna.BitisTarihi = dto.BitisTarihi;
    }
}
