using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class OdevFormViewModel
{
    public OdevKaydetDTO Odev { get; set; } = new();

    public List<OgrenciDTO> OgrenciSecenekleri { get; set; } = [];

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
