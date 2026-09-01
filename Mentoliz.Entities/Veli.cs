using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Veli : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public Yakinlik Yakinlik { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    public string Telefon { get; set; } = string.Empty;

    public string? Meslek { get; set; }

    public string? Eposta { get; set; }

    // raporlarda ve aramalarda önce hangi veliye ulaşılacağını belirtiyor
    public bool BirincilIletisimMi { get; set; }
}
