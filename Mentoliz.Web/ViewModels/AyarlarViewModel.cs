using Mentoliz.Business.Dto.Ayar;

namespace Mentoliz.Web.ViewModels;

public class AyarlarViewModel
{
    public string? KurumAdi { get; set; }

    public string? OgretmenAdi { get; set; }

    public string? YedekKlasoru { get; set; }

    public bool KapanistaOtomatikYedek { get; set; }

    public string SeciliTema { get; set; } = string.Empty;

    public List<YedekDosyaDTO> Yedekler { get; set; } = [];
}
