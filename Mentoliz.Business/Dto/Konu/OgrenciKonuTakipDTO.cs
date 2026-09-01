using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Konu;

public class OgrenciKonuTakipDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public int KonuId { get; set; }

    public string KonuAdi { get; set; } = string.Empty;

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public KonuTakipDurumu Durum { get; set; }

    public DateTime GuncellemeTarihi { get; set; }

    public string? Notlar { get; set; }
}
