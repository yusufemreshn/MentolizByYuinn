using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class DenemeDetayViewModel
{
    public DenemeDTO Deneme { get; set; } = new();

    public SinavTuruDTO SinavTuru { get; set; } = new();

    public List<DenemeSonucDTO> Sonuclar { get; set; } = [];

    // sonuç girişine geçerken sınıf seçim kutusunu doldurmak için
    public List<SinifDTO> SinifSecenekleri { get; set; } = [];
}
