using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class OgrenciListeViewModel
{
    public List<OgrenciDTO> Ogrenciler { get; set; } = [];

    // filtre kutusundaki sınıf seçeneklerini doldurmak için
    public List<SinifDTO> SinifSecenekleri { get; set; } = [];

    // arama ve filtre kutularının sayfa yenilendiğinde değerini korumak için
    public string? Arama { get; set; }

    public int? SinifId { get; set; }

    public bool? AktifMi { get; set; }
}
