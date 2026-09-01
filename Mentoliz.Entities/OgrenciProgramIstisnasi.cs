using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// öğrenciye özel program eklemeleri ve çıkarmaları, sınıf şablonunun üzerine bu uygulanıyor
public class OgrenciProgramIstisnasi : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    // iptal türünde ders seçilmeyebilir, sadece o saatteki sınıf dersi düşülür
    public int? DersId { get; set; }
    public Ders? Ders { get; set; }

    public ProgramIstisnaTuru Tur { get; set; }

    public string? OgretmenAdi { get; set; }

    public string Aciklama { get; set; } = string.Empty;

    // istisna belirli bir tarih aralığında geçerliyse doldurulur, boşsa süresiz sayılır
    public DateTime? BaslangicTarihi { get; set; }

    public DateTime? BitisTarihi { get; set; }
}
