using Mentoliz.Business.Dto.CalismaProgrami;

namespace Mentoliz.Web.ViewModels;

public class CalismaProgramiDetayViewModel
{
    public CalismaProgramiDTO Program { get; set; } = new();

    public string OgrenciAdSoyad { get; set; } = string.Empty;
}
