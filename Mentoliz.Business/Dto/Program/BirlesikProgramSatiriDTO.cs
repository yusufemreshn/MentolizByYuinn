using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Program;

// sınıf ders programı şablonu ile öğrencinin istisnaları birleştirilince ortaya çıkan tek bir satır
public class BirlesikProgramSatiriDTO
{
    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public string? OgretmenAdi { get; set; }

    public string? DerslikAdi { get; set; }

    // boşsa sınıf programından geliyor demek, doluysa öğrenciye özel bir istisnadan geliyor demek
    public ProgramIstisnaTuru? Tur { get; set; }
}
