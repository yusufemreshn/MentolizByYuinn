using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// öğrencinin evde uygulayacağı haftalık plan, kurumdaki ders programından tamamen ayrı bir yapı
public class CalismaProgrami : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public DateTime HaftaBaslangicTarihi { get; set; }

    public string? Aciklama { get; set; }

    public ICollection<CalismaProgramiSatiri> Satirlar { get; set; } = new List<CalismaProgramiSatiri>();
}
