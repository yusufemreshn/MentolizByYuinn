using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Sinif;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Web.ViewModels;

public class OdevListeViewModel
{
    public List<OdevDTO> Odevler { get; set; } = [];

    public List<OdevTopluAtamaDTO> TopluAtamalar { get; set; } = [];

    public List<SinifDTO> SinifSecenekleri { get; set; } = [];

    public List<DersDTO> DersSecenekleri { get; set; } = [];

    // filtre kutularının sayfa yenilendiğinde değerini korumak için
    public string? Arama { get; set; }

    public int? SinifId { get; set; }

    public int? DersId { get; set; }

    public OdevDurumu? Durum { get; set; }
}
