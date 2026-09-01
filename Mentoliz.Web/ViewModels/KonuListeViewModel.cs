using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Konu;

namespace Mentoliz.Web.ViewModels;

public class KonuListeViewModel
{
    public List<KonuDTO> Konular { get; set; } = [];

    public List<DersDTO> DersSecenekleri { get; set; } = [];

    public int? DersId { get; set; }
}
