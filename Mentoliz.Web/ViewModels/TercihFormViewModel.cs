using Mentoliz.Business.Dto.Tercih;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class TercihFormViewModel
{
    public OgrenciTercihiKaydetDTO Tercih { get; set; } = new();

    // formun başlığında hangi öğrenci için düzenlendiğini göstermek için
    public string OgrenciAdSoyad { get; set; } = string.Empty;
}
