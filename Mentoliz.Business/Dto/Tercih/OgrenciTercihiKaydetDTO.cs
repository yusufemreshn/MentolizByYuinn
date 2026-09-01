namespace Mentoliz.Business.Dto.Tercih;

public class OgrenciTercihiKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public int Sira { get; set; }

    public string UniversiteAdi { get; set; } = string.Empty;

    public string BolumAdi { get; set; } = string.Empty;

    public decimal? TabanPuan { get; set; }

    public string? Notlar { get; set; }
}
