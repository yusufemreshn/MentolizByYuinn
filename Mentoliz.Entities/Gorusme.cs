using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// birebir görüşme kaydı, buradaki notlar veliye verilecek rapora kesinlikle girmeyecek
public class Gorusme : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public GorusmeTuru Tur { get; set; }

    public string Konu { get; set; } = string.Empty;

    public string Notlar { get; set; } = string.Empty;

    public string? AlinanKarar { get; set; }

    // burası doldurulursa otomatik olarak bir görev oluşturulacak
    public DateTime? SonrakiGorusmeTarihi { get; set; }

    public int? Sure { get; set; }
}
