using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class GorevListeViewModel
{
    public List<GorevDTO> Gorevler { get; set; } = [];

    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public int? OgrenciId { get; set; }

    public bool? TamamlandiMi { get; set; }
}
