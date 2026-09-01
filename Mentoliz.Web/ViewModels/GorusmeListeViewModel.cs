using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Web.ViewModels;

public class GorusmeListeViewModel
{
    public List<GorusmeDTO> Gorusmeler { get; set; } = [];

    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public int? OgrenciId { get; set; }

    public GorusmeTuru? Tur { get; set; }
}
