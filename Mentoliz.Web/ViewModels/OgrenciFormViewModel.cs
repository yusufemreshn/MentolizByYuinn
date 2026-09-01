using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class OgrenciFormViewModel
{
    public OgrenciKaydetDTO Ogrenci { get; set; } = new();

    public List<SinifDTO> SinifSecenekleri { get; set; } = [];
}
