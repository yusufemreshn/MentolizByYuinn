using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class DevamsizlikEslemeleri
{
    public static OgrenciDevamsizlik Entity(this DevamsizlikKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        Tarih = dto.Tarih.Date,
        Durum = dto.Durum,
        Not = dto.Not
    };

    public static void Uygula(this DevamsizlikKaydetDTO dto, OgrenciDevamsizlik devamsizlik)
    {
        devamsizlik.Durum = dto.Durum;
        devamsizlik.Not = dto.Not;
    }
}
