using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// öğrencinin bir güne ait devamsızlık kaydı, o gün için kayıt hiç yoksa öğrenci geldi sayılıyor, sadece istisnalar burada tutuluyor
public class OgrenciDevamsizlik : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public DevamsizlikDurumu Durum { get; set; }

    public string? Not { get; set; }
}
