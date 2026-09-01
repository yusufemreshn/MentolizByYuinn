using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// müfredat konu listesi, başlangıçta örnek konularla tohumlanacak ama öğretmen sonradan düzenleyebilecek
public class Konu : BaseEntity
{
    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public string Ad { get; set; } = string.Empty;

    public KonuSeviyesi Seviye { get; set; }

    // konu listesinde hangi sırada görünecek, genelde müfredat sırasını takip eder
    public int Sira { get; set; }

    public bool AktifMi { get; set; }

    public ICollection<OgrenciKonuTakip> OgrenciTakipleri { get; set; } = new List<OgrenciKonuTakip>();
}
