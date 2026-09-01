using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class DonemRaporuViewModel
{
    public OgrenciDTO Ogrenci { get; set; } = new();

    public DonemDegerlendirmeDTO Degerlendirme { get; set; } = new();

    public DateTime OlusturmaTarihi { get; set; }
}
