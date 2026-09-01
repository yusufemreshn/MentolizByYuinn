using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class DersProgramiFormViewModel
{
    public DersProgramiSatiriKaydetDTO Satir { get; set; } = new();

    // formun başlığında hangi sınıfa saat eklendiğini göstermek için
    public string SinifAdi { get; set; } = string.Empty;

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
