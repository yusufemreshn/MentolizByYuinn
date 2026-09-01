using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// tyt, ayt sayısal gibi sınav türleri, ileride lgs gibi yeni türler kod değişmeden buraya eklenebilsin diye tablo yapıldı
public class SinavTuru : BaseEntity
{
    public string Ad { get; set; } = string.Empty;

    public string Kod { get; set; } = string.Empty;

    public int Sira { get; set; }

    public bool AktifMi { get; set; }

    // bu sınav türünde hangi dersten kaç soru olduğunu tanımlayan satırlar
    public ICollection<SinavTuruTest> Testler { get; set; } = new List<SinavTuruTest>();

    public ICollection<Deneme> Denemeler { get; set; } = new List<Deneme>();
}
