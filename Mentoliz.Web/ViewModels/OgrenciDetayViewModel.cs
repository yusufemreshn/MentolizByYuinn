using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Business.Dto.Konu;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Business.Dto.Tercih;

namespace Mentoliz.Web.ViewModels;

public class OgrenciDetayViewModel
{
    public OgrenciDTO Ogrenci { get; set; } = new();

    public List<DersIlerlemeDTO> KonuIlerlemeleri { get; set; } = [];

    // en güncel haftaya ait çalışma programı, yoksa boş
    public CalismaProgramiDTO? GuncelCalismaProgrami { get; set; }

    public List<GorusmeDTO> Gorusmeler { get; set; } = [];

    public HedefDTO? AktifHedef { get; set; }

    public HedefKarsilastirmaDTO? HedefKarsilastirma { get; set; }

    public List<OdevDTO> Odevler { get; set; } = [];

    public OgrenciOdevOzetiDTO OdevOzeti { get; set; } = new();

    // deneme sonuçları sekmesindeki sınav türü seçimi sekmelerini doldurmak için
    public List<SinavTuruDTO> KatildigiSinavTurleri { get; set; } = [];

    public int? SeciliSinavTuruId { get; set; }

    public OgrenciDenemeAnaliziDTO DenemeAnalizi { get; set; } = new();

    // sınıf şablonu ile istisnaların birleştirilmiş hali, salt okunur bir ızgarada gösteriliyor
    public List<BirlesikProgramSatiriDTO> BirlesikProgram { get; set; } = [];

    // birleşik programdan hesaplanan günlük kurumda bulunma özeti
    public List<HaftalikBulunmaSuresiDTO> HaftalikBulunmaSuresi { get; set; } = [];

    public List<OgrenciProgramIstisnasiDTO> Istisnalar { get; set; } = [];

    // görüşme, ödev, deneme ve görev kayıtlarının tek kronolojik akışı, zaman tüneli sekmesinde gösteriliyor
    public List<ZamanTuneliOgesiDTO> ZamanTuneli { get; set; } = [];

    // son deneme sonuçlarında zayıf çıkan derslerden önerilen konular, konu takip sekmesinde deneme analiziyle bağlantı kuruyor
    public List<KonuDTO> ZayifKonuOnerileri { get; set; } = [];

    public List<OgrenciTercihiDTO> Tercihler { get; set; } = [];

    // son otuz güne ait devamsızlık özeti, genel bilgiler sekmesinde gösteriliyor
    public OgrenciDevamsizlikOzetiDTO DevamsizlikOzeti { get; set; } = new();
}
