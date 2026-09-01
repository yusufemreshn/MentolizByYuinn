using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// sınıfın haftalık ders programındaki tek bir satır, öğrenci bu sınıfa atanınca otomatik olarak onun programı sayılıyor
public class DersProgramiSatiri : BaseEntity
{
    public int SinifId { get; set; }
    public Sinif Sinif { get; set; } = null!;

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public string? DerslikAdi { get; set; }

    public string? OgretmenAdi { get; set; }
}
