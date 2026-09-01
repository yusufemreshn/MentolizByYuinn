using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;

namespace Mentoliz.Web.ViewModels;

// öğretmenin gün içinde tek ekrandan işleyebileceği üç liste bir arada
public class BugunIndexViewModel
{
    public List<GorevDTO> BugunkuGorevler { get; set; } = [];

    public List<OdevDTO> BugunTeslimOdevler { get; set; } = [];

    // altmış günden uzun süredir görüşülmeyen veya hiç görüşülmemiş öğrenciler
    public List<UzunSureGorusulmeyenOgrenciDTO> IhmalEdilenOgrenciler { get; set; } = [];
}
