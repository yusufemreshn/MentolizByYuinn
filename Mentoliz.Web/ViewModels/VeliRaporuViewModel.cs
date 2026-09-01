using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Web.ViewModels;

// görüşme notları kesinlikle bu rapora girmeyeceği için görüşme sekmesine hiç yer verilmiyor
public class VeliRaporuViewModel
{
    public OgrenciDTO Ogrenci { get; set; } = new();

    public OgrenciDenemeAnaliziDTO DenemeAnalizi { get; set; } = new();

    public OgrenciOdevOzetiDTO OdevOzeti { get; set; } = new();

    public List<HaftalikBulunmaSuresiDTO> HaftalikBulunmaSuresi { get; set; } = [];

    public DateTime OlusturmaTarihi { get; set; }

    // veliye kopyala yapıştır ile gönderilebilecek hazır metin, yazdırılan rapora dahil değil
    public string VeliMesajiMetni { get; set; } = string.Empty;
}
