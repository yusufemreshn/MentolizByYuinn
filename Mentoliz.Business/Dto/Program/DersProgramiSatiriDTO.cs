using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Program;

public class DersProgramiSatiriDTO
{
    public int Id { get; set; }

    public int SinifId { get; set; }

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public string? DerslikAdi { get; set; }

    public string? OgretmenAdi { get; set; }
}
