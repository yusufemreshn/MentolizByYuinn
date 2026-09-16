using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class DevamsizlikIndexViewModel
{
    public List<SinifDTO> SinifSecenekleri { get; set; } = [];

    public int? SinifId { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Today;

    public SinifDevamsizlikGunuDTO? Gun { get; set; }
}
