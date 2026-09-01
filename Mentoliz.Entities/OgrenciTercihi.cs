using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// tercih dönemi geldiğinde öğrencinin hedeflediği üniversite ve bölüm sıralaması, sıraya göre listeleniyor
public class OgrenciTercihi : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    // tercih listesindeki sırası, birinci tercih bir olur
    public int Sira { get; set; }

    public string UniversiteAdi { get; set; } = string.Empty;

    public string BolumAdi { get; set; } = string.Empty;

    // taban puan her yıl değiştiği için sadece referans amaçlı not olarak tutuluyor, hesaplamada kullanılmıyor
    public decimal? TabanPuan { get; set; }

    public string? Notlar { get; set; }
}
