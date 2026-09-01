using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class TopluAtamaFormViewModel
{
    public OdevTopluAtamaKaydetDTO TopluAtama { get; set; } = new();

    public List<SinifDTO> SinifSecenekleri { get; set; } = [];

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
