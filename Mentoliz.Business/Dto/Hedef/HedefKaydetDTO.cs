using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Hedef;

public class HedefKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public string HedefUniversite { get; set; } = string.Empty;

    public string HedefBolum { get; set; } = string.Empty;

    public PuanTuru PuanTuru { get; set; }

    public int? HedefSiralama { get; set; }

    public decimal? HedefToplamNet { get; set; }

    public List<HedefDersNetiKaydetDTO> DersNetleri { get; set; } = [];
}
