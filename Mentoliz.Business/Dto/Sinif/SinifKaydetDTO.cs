using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Sinif;

public class SinifKaydetDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public SinifSeviyesi Seviye { get; set; }

    public Alan Alan { get; set; }

    public int Kontenjan { get; set; }

    public bool AktifMi { get; set; }
}
