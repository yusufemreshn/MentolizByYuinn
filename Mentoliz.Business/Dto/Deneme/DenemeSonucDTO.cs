namespace Mentoliz.Business.Dto.Deneme;

public class DenemeSonucDTO
{
    public int Id { get; set; }

    public int DenemeId { get; set; }

    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    public string? Notlar { get; set; }

    public bool KatilmadiMi { get; set; }

    public List<DenemeSonucDetayDTO> Detaylar { get; set; } = [];

    // veritabanında tutulmuyor, detaylardaki netlerin toplamından hesaplanıyor
    public decimal ToplamNet { get; set; }
}
