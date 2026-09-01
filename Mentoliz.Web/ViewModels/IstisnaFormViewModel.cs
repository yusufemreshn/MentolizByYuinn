using Mentoliz.Business.Dto.Ders;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class IstisnaFormViewModel
{
    public OgrenciProgramIstisnasiKaydetDTO Istisna { get; set; } = new();

    // formun başlığında hangi öğrenci için düzenlendiğini göstermek için
    public string OgrenciAdSoyad { get; set; } = string.Empty;

    public List<DersDTO> DersSecenekleri { get; set; } = [];
}
