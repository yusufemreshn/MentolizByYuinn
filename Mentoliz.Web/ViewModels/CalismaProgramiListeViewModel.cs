using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class CalismaProgramiListeViewModel
{
    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public int? OgrenciId { get; set; }

    public List<CalismaProgramiDTO> Programlar { get; set; } = [];
}
