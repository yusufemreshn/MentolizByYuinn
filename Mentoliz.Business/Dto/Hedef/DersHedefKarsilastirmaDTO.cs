namespace Mentoliz.Business.Dto.Hedef;

// hedef net ile en son denemedeki güncel netin karşılaştırılmış hali
public class DersHedefKarsilastirmaDTO
{
    public string DersAdi { get; set; } = string.Empty;

    public decimal HedefNet { get; set; }

    // bu derste hiç deneme sonucu girilmemişse boş kalır
    public decimal? GuncelNet { get; set; }

    // pozitifse hedef geçilmiş, negatifse hedefe daha uzak demek
    public decimal? Uzaklik { get; set; }
}
