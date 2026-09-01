using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class HedefIndexViewModel
{
    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public int? OgrenciId { get; set; }

    public HedefDTO? AktifHedef { get; set; }

    public HedefKarsilastirmaDTO? Karsilastirma { get; set; }

    // aktif olan hariç geçmiş hedefler
    public List<HedefDTO> Gecmis { get; set; } = [];
}
