namespace Mentoliz.Business.Dto.CalismaProgrami;

public class CalismaProgramiKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public DateTime HaftaBaslangicTarihi { get; set; }

    public string? Aciklama { get; set; }

    public List<CalismaProgramiSatiriKaydetDTO> Satirlar { get; set; } = [];
}
