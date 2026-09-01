using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Ders;

public class DersDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string KisaAd { get; set; } = string.Empty;

    public DersKategori Kategori { get; set; }

    public int Sira { get; set; }

    public bool AktifMi { get; set; }
}
