using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Deneme : BaseEntity
{
    public string Ad { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }

    public int SinavTuruId { get; set; }
    public SinavTuru SinavTuru { get; set; } = null!;

    public string? YayinAdi { get; set; }

    public string? Aciklama { get; set; }

    public ICollection<DenemeSonuc> Sonuclar { get; set; } = new List<DenemeSonuc>();
}
