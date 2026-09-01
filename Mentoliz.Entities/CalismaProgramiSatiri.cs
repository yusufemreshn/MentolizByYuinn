using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class CalismaProgramiSatiri : BaseEntity
{
    public int CalismaProgramiId { get; set; }
    public CalismaProgrami CalismaProgrami { get; set; } = null!;

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    // serbest çalışma saati olabilir diye ders seçimi zorunlu tutulmadı
    public int? DersId { get; set; }
    public Ders? Ders { get; set; }

    public string Aciklama { get; set; } = string.Empty;

    // öğrenci bu satırı gerçekleştirdiyse işaretleniyor, haftalık gerçekleşme oranı buradan hesaplanacak
    public bool TamamlandiMi { get; set; }
}
