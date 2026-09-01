using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class GorusmeFormViewModel
{
    public GorusmeKaydetDTO Gorusme { get; set; } = new();

    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];
}
