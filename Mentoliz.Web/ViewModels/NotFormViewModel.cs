using Mentoliz.Business.Dto.Not;

namespace Mentoliz.Web.ViewModels;

// ekleme ve düzenleme formu aynı görünümü ve aynı viewmodel'i paylaşıyor
public class NotFormViewModel
{
    public NotKaydetDTO Not { get; set; } = new();
}
