namespace Mentoliz.Business.Dto.Deneme;

public class DenemeSonucKaydetDTO
{
    public int Id { get; set; }

    public int DenemeId { get; set; }

    public int OgrenciId { get; set; }

    public string? Notlar { get; set; }

    public bool KatilmadiMi { get; set; }

    public List<DenemeSonucDetayKaydetDTO> Detaylar { get; set; } = [];
}
