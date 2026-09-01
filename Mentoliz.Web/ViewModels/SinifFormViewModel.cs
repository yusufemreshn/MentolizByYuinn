using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class SinifFormViewModel
{
    public SinifKaydetDTO Sinif { get; set; } = new();
}
