using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class GorusmeEslemeleri
{
    public static GorusmeDTO Dto(this Gorusme gorusme) => new()
    {
        Id = gorusme.Id,
        OgrenciId = gorusme.OgrenciId,
        OgrenciAdSoyad = gorusme.Ogrenci is null ? string.Empty : $"{gorusme.Ogrenci.Ad} {gorusme.Ogrenci.Soyad}",
        Tarih = gorusme.Tarih,
        Tur = gorusme.Tur,
        Konu = gorusme.Konu,
        Notlar = gorusme.Notlar,
        AlinanKarar = gorusme.AlinanKarar,
        SonrakiGorusmeTarihi = gorusme.SonrakiGorusmeTarihi,
        Sure = gorusme.Sure
    };

    public static GorusmeKaydetDTO KaydetDto(this Gorusme gorusme) => new()
    {
        Id = gorusme.Id,
        OgrenciId = gorusme.OgrenciId,
        Tarih = gorusme.Tarih,
        Tur = gorusme.Tur,
        Konu = gorusme.Konu,
        Notlar = gorusme.Notlar,
        AlinanKarar = gorusme.AlinanKarar,
        SonrakiGorusmeTarihi = gorusme.SonrakiGorusmeTarihi,
        Sure = gorusme.Sure
    };

    public static Gorusme Entity(this GorusmeKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        Tarih = dto.Tarih,
        Tur = dto.Tur,
        Konu = dto.Konu,
        Notlar = dto.Notlar,
        AlinanKarar = dto.AlinanKarar,
        SonrakiGorusmeTarihi = dto.SonrakiGorusmeTarihi,
        Sure = dto.Sure
    };

    public static void Uygula(this GorusmeKaydetDTO dto, Gorusme gorusme)
    {
        gorusme.OgrenciId = dto.OgrenciId;
        gorusme.Tarih = dto.Tarih;
        gorusme.Tur = dto.Tur;
        gorusme.Konu = dto.Konu;
        gorusme.Notlar = dto.Notlar;
        gorusme.AlinanKarar = dto.AlinanKarar;
        gorusme.SonrakiGorusmeTarihi = dto.SonrakiGorusmeTarihi;
        gorusme.Sure = dto.Sure;
    }
}
