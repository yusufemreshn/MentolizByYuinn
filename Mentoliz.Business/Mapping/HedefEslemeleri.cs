using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class HedefEslemeleri
{
    public static HedefDersNetiDTO Dto(this HedefDersNeti dersNeti) => new()
    {
        Id = dersNeti.Id,
        DersId = dersNeti.DersId,
        DersAdi = dersNeti.Ders?.Ad ?? string.Empty,
        HedefNet = dersNeti.HedefNet
    };

    public static HedefDersNetiKaydetDTO KaydetDto(this HedefDersNeti dersNeti) => new()
    {
        Id = dersNeti.Id,
        DersId = dersNeti.DersId,
        HedefNet = dersNeti.HedefNet
    };

    public static HedefDersNeti Entity(this HedefDersNetiKaydetDTO dto) => new()
    {
        DersId = dto.DersId,
        HedefNet = dto.HedefNet
    };

    public static void Uygula(this HedefDersNetiKaydetDTO dto, HedefDersNeti dersNeti)
    {
        dersNeti.DersId = dto.DersId;
        dersNeti.HedefNet = dto.HedefNet;
    }

    public static HedefDTO Dto(this Hedef hedef) => new()
    {
        Id = hedef.Id,
        OgrenciId = hedef.OgrenciId,
        HedefUniversite = hedef.HedefUniversite,
        HedefBolum = hedef.HedefBolum,
        PuanTuru = hedef.PuanTuru,
        HedefSiralama = hedef.HedefSiralama,
        HedefToplamNet = hedef.HedefToplamNet,
        OlusturmaTarihi = hedef.OlusturmaTarihi,
        AktifMi = hedef.AktifMi,
        DersNetleri = hedef.DersNetleri.Select(d => d.Dto()).ToList()
    };

    public static HedefKaydetDTO KaydetDto(this Hedef hedef) => new()
    {
        Id = hedef.Id,
        OgrenciId = hedef.OgrenciId,
        HedefUniversite = hedef.HedefUniversite,
        HedefBolum = hedef.HedefBolum,
        PuanTuru = hedef.PuanTuru,
        HedefSiralama = hedef.HedefSiralama,
        HedefToplamNet = hedef.HedefToplamNet,
        DersNetleri = hedef.DersNetleri.Select(d => d.KaydetDto()).ToList()
    };

    public static Hedef Entity(this HedefKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        HedefUniversite = dto.HedefUniversite,
        HedefBolum = dto.HedefBolum,
        PuanTuru = dto.PuanTuru,
        HedefSiralama = dto.HedefSiralama,
        HedefToplamNet = dto.HedefToplamNet
    };

    public static void Uygula(this HedefKaydetDTO dto, Hedef hedef)
    {
        hedef.HedefUniversite = dto.HedefUniversite;
        hedef.HedefBolum = dto.HedefBolum;
        hedef.PuanTuru = dto.PuanTuru;
        hedef.HedefSiralama = dto.HedefSiralama;
        hedef.HedefToplamNet = dto.HedefToplamNet;
    }
}
