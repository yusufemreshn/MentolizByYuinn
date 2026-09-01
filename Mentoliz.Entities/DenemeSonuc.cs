using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// bir öğrencinin bir denemeye ait sonucu, test bazlı detaylar ayrı tabloda
public class DenemeSonuc : BaseEntity
{
    public int DenemeId { get; set; }
    public Deneme Deneme { get; set; } = null!;

    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public string? Notlar { get; set; }

    // öğrenci denemeye girmediyse net hesabına dahil edilmesin diye bu alan var
    public bool KatilmadiMi { get; set; }

    public ICollection<DenemeSonucDetay> Detaylar { get; set; } = new List<DenemeSonucDetay>();
}
