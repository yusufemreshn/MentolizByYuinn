using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

public class HomeIndexViewModel
{
    public int ToplamOgrenciSayisi { get; set; }

    public int BugunGorusulecekSayisi { get; set; }

    public int GecikenOdevSayisi { get; set; }

    public int BuHaftaGirilenDenemeSayisi { get; set; }

    public int HaftalikOdevTamamlamaOrani { get; set; }

    public List<OdevDTO> YaklasanTeslimTarihleri { get; set; } = [];

    public List<GorevDTO> BugunkuGorevler { get; set; } = [];

    public List<UzunSureGorusulmeyenOgrenciDTO> UzunSureGorusulmeyenler { get; set; } = [];

    public DenemeAnaliziSonucuDTO? SinifOrtalamaNetSonucu { get; set; }

    public OgrenciNetDegisimiOzetiDTO SonDenemeNetDegisimOzeti { get; set; } = new();

    // en yüksek riskli aktif öğrenciler, ana sayfada gösterilecek kadarı önceden kırpılmış olarak geliyor
    public List<OgrenciRiskDTO> DikkatGerektirenOgrenciler { get; set; } = [];
}
