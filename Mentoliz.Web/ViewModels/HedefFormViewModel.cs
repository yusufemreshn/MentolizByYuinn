using Mentoliz.Business.Dto.Hedef;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class HedefFormViewModel
{
    public HedefKaydetDTO Hedef { get; set; } = new();

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    public List<HedefDersNetiSatiriViewModel> DersNetSatirlari { get; set; } = [];
}
