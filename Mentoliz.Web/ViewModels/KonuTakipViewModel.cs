using Mentoliz.Business.Dto.Konu;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class KonuTakipViewModel
{
    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public int? OgrenciId { get; set; }

    // ısı haritası ızgarası için, ders bazlı gruplanarak gösteriliyor
    public List<OgrenciKonuTakipDTO> Takipler { get; set; } = [];

    public List<DersIlerlemeDTO> DersIlerlemeleri { get; set; } = [];

    public List<KonuDTO> ZayifKonuOnerileri { get; set; } = [];
}
