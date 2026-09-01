using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class DenemeFormViewModel
{
    public DenemeKaydetDTO Deneme { get; set; } = new();

    public List<SinavTuruDTO> SinavTuruSecenekleri { get; set; } = [];
}
