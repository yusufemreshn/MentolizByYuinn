using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Konu;

public class KonuKaydetDTO
{
    public int Id { get; set; }

    public int DersId { get; set; }

    public string Ad { get; set; } = string.Empty;

    public KonuSeviyesi Seviye { get; set; }

    public int Sira { get; set; }

    public bool AktifMi { get; set; }
}
