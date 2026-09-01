using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Gorusme;

public class GorusmeKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public DateTime Tarih { get; set; }

    public GorusmeTuru Tur { get; set; }

    public string Konu { get; set; } = string.Empty;

    public string Notlar { get; set; } = string.Empty;

    public string? AlinanKarar { get; set; }

    public DateTime? SonrakiGorusmeTarihi { get; set; }

    public int? Sure { get; set; }
}
