using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Business.Dto.Ders;

namespace Mentoliz.Web.ViewModels;

public class CalismaSatirFormViewModel
{
    public int CalismaProgramiId { get; set; }

    public CalismaProgramiSatiriKaydetDTO Satir { get; set; } = new();

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
