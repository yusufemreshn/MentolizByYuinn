using Mentoliz.Business.Dto.Ayar;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class AyarEslemeleri
{
    public static AyarDTO Dto(this Ayar ayar) => new()
    {
        Id = ayar.Id,
        Anahtar = ayar.Anahtar,
        Deger = ayar.Deger
    };
}
