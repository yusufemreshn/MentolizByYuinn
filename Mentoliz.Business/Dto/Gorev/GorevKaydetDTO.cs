using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Gorev;

public class GorevKaydetDTO
{
    public int Id { get; set; }

    public string Baslik { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    public int? OgrenciId { get; set; }

    public GorevOnceligi Oncelik { get; set; }

    public bool TamamlandiMi { get; set; }
}
