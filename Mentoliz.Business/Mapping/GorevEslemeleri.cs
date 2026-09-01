using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class GorevEslemeleri
{
    public static GorevDTO Dto(this Gorev gorev) => new()
    {
        Id = gorev.Id,
        Baslik = gorev.Baslik,
        Aciklama = gorev.Aciklama,
        Tarih = gorev.Tarih,
        OgrenciId = gorev.OgrenciId,
        OgrenciAdSoyad = gorev.Ogrenci is null ? null : $"{gorev.Ogrenci.Ad} {gorev.Ogrenci.Soyad}",
        Oncelik = gorev.Oncelik,
        TamamlandiMi = gorev.TamamlandiMi
    };

    public static GorevKaydetDTO KaydetDto(this Gorev gorev) => new()
    {
        Id = gorev.Id,
        Baslik = gorev.Baslik,
        Aciklama = gorev.Aciklama,
        Tarih = gorev.Tarih,
        OgrenciId = gorev.OgrenciId,
        Oncelik = gorev.Oncelik,
        TamamlandiMi = gorev.TamamlandiMi
    };

    public static Gorev Entity(this GorevKaydetDTO dto) => new()
    {
        Baslik = dto.Baslik,
        Aciklama = dto.Aciklama,
        Tarih = dto.Tarih,
        OgrenciId = dto.OgrenciId,
        Oncelik = dto.Oncelik,
        TamamlandiMi = dto.TamamlandiMi
    };

    public static void Uygula(this GorevKaydetDTO dto, Gorev gorev)
    {
        gorev.Baslik = dto.Baslik;
        gorev.Aciklama = dto.Aciklama;
        gorev.Tarih = dto.Tarih;
        gorev.OgrenciId = dto.OgrenciId;
        gorev.Oncelik = dto.Oncelik;
        gorev.TamamlandiMi = dto.TamamlandiMi;
    }
}
