using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class SonucGirisiViewModel
{
    public DenemeDTO Deneme { get; set; } = new();

    public SinavTuruDTO SinavTuru { get; set; } = new();

    public List<SinifDTO> SinifSecenekleri { get; set; } = [];

    public int? SinifId { get; set; }

    public List<DenemeSonucSatiriDTO> Satirlar { get; set; } = [];
}
