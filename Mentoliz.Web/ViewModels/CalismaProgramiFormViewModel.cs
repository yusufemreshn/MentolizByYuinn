using Mentoliz.Business.Dto.CalismaProgrami;

namespace Mentoliz.Web.ViewModels;

public class CalismaProgramiFormViewModel
{
    public CalismaProgramiKaydetDTO Program { get; set; } = new();

    public string OgrenciAdSoyad { get; set; } = string.Empty;
}
