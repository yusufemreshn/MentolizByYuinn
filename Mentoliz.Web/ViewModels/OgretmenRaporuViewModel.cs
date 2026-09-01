using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Business.Dto.Konu;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.ViewModels;

// öğretmen raporunda görüşme notları da dahil bütün detaylar gösterilir, veli raporundan farkı budur
public class OgretmenRaporuViewModel
{
    public OgrenciDTO Ogrenci { get; set; } = new();

    public OgrenciDenemeAnaliziDTO DenemeAnalizi { get; set; } = new();

    public OgrenciOdevOzetiDTO OdevOzeti { get; set; } = new();

    public List<DersIlerlemeDTO> KonuIlerlemeleri { get; set; } = [];

    public HedefKarsilastirmaDTO? HedefKarsilastirma { get; set; }

    public List<GorusmeDTO> Gorusmeler { get; set; } = [];

    public DateTime OlusturmaTarihi { get; set; }
}
