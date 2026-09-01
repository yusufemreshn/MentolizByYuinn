using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Web.ViewModels;

public class SinifDetayViewModel
{
    public SinifDTO Sinif { get; set; } = new();

    // bu sınıfa atanmış öğrenciler
    public List<OgrenciDTO> SinifOgrencileri { get; set; } = [];

    // öğrenci atama kutusunu doldurmak için bütün aktif öğrenciler geliyor, sınıfta olanlar view tarafında elenip gösteriliyor
    public List<OgrenciDTO> AktifOgrenciler { get; set; } = [];

    public List<DersProgramiSatiriDTO> DersProgrami { get; set; } = [];
}
