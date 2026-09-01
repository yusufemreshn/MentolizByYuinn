using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.CalismaProgrami;

public class CalismaProgramiSatiriKaydetDTO
{
    public int Id { get; set; }

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    public int? DersId { get; set; }

    public string Aciklama { get; set; } = string.Empty;

    public bool TamamlandiMi { get; set; }
}
