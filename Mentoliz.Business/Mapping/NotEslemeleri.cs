using Mentoliz.Business.Dto.Not;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class NotEslemeleri
{
    public static NotDTO Dto(this Not not) => new()
    {
        Id = not.Id,
        Baslik = not.Baslik,
        Icerik = not.Icerik,
        OlusturmaTarihi = not.OlusturmaTarihi,
        GuncellemeTarihi = not.GuncellemeTarihi
    };

    public static NotKaydetDTO KaydetDto(this Not not) => new()
    {
        Id = not.Id,
        Baslik = not.Baslik,
        Icerik = not.Icerik
    };

    public static Not Entity(this NotKaydetDTO dto) => new()
    {
        Baslik = dto.Baslik,
        Icerik = dto.Icerik
    };

    public static void Uygula(this NotKaydetDTO dto, Not not)
    {
        not.Baslik = dto.Baslik;
        not.Icerik = dto.Icerik;
    }
}
