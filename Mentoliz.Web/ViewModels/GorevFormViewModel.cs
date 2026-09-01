using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class GorevFormViewModel
{
    public GorevKaydetDTO Gorev { get; set; } = new();

    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];
}
