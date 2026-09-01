using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Sinif : BaseEntity
{
    // örneğin on iki a gibi
    public string Ad { get; set; } = string.Empty;

    public SinifSeviyesi Seviye { get; set; }

    public Alan Alan { get; set; }

    // sınıfın alabileceği en fazla öğrenci sayısı
    public int Kontenjan { get; set; }

    // kapanan sınıflar burada false olur, geçmiş dönem kayıtları için silinmez
    public bool AktifMi { get; set; }

    public ICollection<Ogrenci> Ogrenciler { get; set; } = new List<Ogrenci>();

    // sınıfın haftalık ders programı şablonu
    public ICollection<DersProgramiSatiri> DersProgramiSatirlari { get; set; } = new List<DersProgramiSatiri>();

    public ICollection<OdevTopluAtama> OdevTopluAtamalari { get; set; } = new List<OdevTopluAtama>();
}
