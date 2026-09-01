using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Konu;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class KonuFormViewModel
{
    public KonuKaydetDTO Konu { get; set; } = new();

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
