using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Ogrenci : BaseEntity
{
    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    // her öğrenciye ulaşamayabiliyoruz, o yüzden zorunlu tutmadık
    public string? Telefon { get; set; }

    // kurumun kendi öğrenci numarası varsa buraya giriliyor, olmayabilir
    public string? OgrenciNo { get; set; }

    // öğrenci kayıt olduğunda henüz bir sınıfa atanmamış olabilir
    public int? SinifId { get; set; }
    public Sinif? Sinif { get; set; }

    public DateTime? DogumTarihi { get; set; }

    public Cinsiyet Cinsiyet { get; set; }

    public string? OkulAdi { get; set; }

    public Alan Alan { get; set; }

    // öğrencinin kuruma ilk kayıt olduğu tarih
    public DateTime KayitTarihi { get; set; }

    // ayrılan öğrenciler burada false olur, listelerde görünmez ama geçmiş verisi durur
    public bool AktifMi { get; set; }

    public string? FotografYolu { get; set; }

    public string? GenelNotlar { get; set; }

    // bir öğrencinin birden fazla velisi olabiliyor
    public ICollection<Veli> Veliler { get; set; } = new List<Veli>();

    public ICollection<OgrenciProgramIstisnasi> ProgramIstisnalari { get; set; } = new List<OgrenciProgramIstisnasi>();

    public ICollection<Odev> Odevler { get; set; } = new List<Odev>();

    public ICollection<DenemeSonuc> DenemeSonuclari { get; set; } = new List<DenemeSonuc>();

    // rehberlik görüşmesi notları, veli raporuna asla dahil edilmeyecek
    public ICollection<Gorusme> Gorusmeler { get; set; } = new List<Gorusme>();

    // geçmiş hedefler de saklanıyor, aynı anda sadece biri aktif oluyor
    public ICollection<Hedef> Hedefler { get; set; } = new List<Hedef>();

    public ICollection<CalismaProgrami> CalismaProgramlari { get; set; } = new List<CalismaProgrami>();

    public ICollection<OgrenciKonuTakip> KonuTakipleri { get; set; } = new List<OgrenciKonuTakip>();

    public ICollection<Gorev> Gorevler { get; set; } = new List<Gorev>();

    public ICollection<OgrenciTercihi> Tercihler { get; set; } = new List<OgrenciTercihi>();
}
