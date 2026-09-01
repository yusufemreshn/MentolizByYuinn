namespace Mentoliz.Business.Dto.CalismaProgrami;

public class CalismaProgramiDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public DateTime HaftaBaslangicTarihi { get; set; }

    public string? Aciklama { get; set; }

    public List<CalismaProgramiSatiriDTO> Satirlar { get; set; } = [];

    // veritabanında tutulmuyor, tamamlanan satır oranından hesaplanıyor
    public int GerceklesmeYuzdesi => Satirlar.Count == 0 ? 0 : (int)Math.Round(Satirlar.Count(s => s.TamamlandiMi) * 100m / Satirlar.Count);
}
