using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Deneme;

// bir alanın hem tyt hem kendi alan sınavındaki (sayısal/sözel/eşit ağırlık için ayt, dil için ydt) sınıf ortalamalarını bir arada tutuyor
public class AlanBazliDenemeAnaliziDTO
{
    public Alan Alan { get; set; }

    public DenemeAnaliziSonucuDTO? TytSonucu { get; set; }

    public DenemeAnaliziSonucuDTO? AlanSinaviSonucu { get; set; }
}
