using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Konu;

public class OgrenciKonuTakipKaydetDTO
{
    public int OgrenciId { get; set; }

    public int KonuId { get; set; }

    public KonuTakipDurumu Durum { get; set; }

    public string? Notlar { get; set; }
}
