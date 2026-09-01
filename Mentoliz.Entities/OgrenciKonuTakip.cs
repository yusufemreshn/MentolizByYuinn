using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class OgrenciKonuTakip : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public int KonuId { get; set; }
    public Konu Konu { get; set; } = null!;

    // durum her değiştiğinde BaseEntity.GuncellemeTarihi zaten yenileniyor, ayrı bir alan açmadık
    public KonuTakipDurumu Durum { get; set; }

    public string? Notlar { get; set; }
}
