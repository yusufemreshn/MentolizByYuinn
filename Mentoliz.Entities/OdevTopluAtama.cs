using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// sınıfa toplu ödev verildiğinde her öğrenci için ayrı odev kaydı oluşuyor ama hepsi buraya bağlanıyor, toplu düzenleme ve silme bu sayede mümkün
public class OdevTopluAtama : BaseEntity
{
    public int SinifId { get; set; }
    public Sinif Sinif { get; set; } = null!;

    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public string Baslik { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime SonTeslimTarihi { get; set; }

    public ICollection<Odev> Odevler { get; set; } = new List<Odev>();
}
