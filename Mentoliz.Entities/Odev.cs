using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Odev : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public string KonuBasligi { get; set; } = string.Empty;

    public string? KaynakAdi { get; set; }

    public int? ToplamSoruSayisi { get; set; }

    public int? TamamlananSoruSayisi { get; set; }

    public DateTime VerilisTarihi { get; set; }

    public DateTime SonTeslimTarihi { get; set; }

    // öğrenci ödevi yapınca doluyor, boşken henüz yapılmamış demek
    public DateTime? YapilmaTarihi { get; set; }

    // elle değiştirilebiliyor ama okuma anında da otomatik güncelleniyor, teslim tarihi geçip yapılma tarihi boşsa gecikti gösteriliyor
    public OdevDurumu Durum { get; set; }

    public string? Aciklama { get; set; }

    // sınıfa toplu verilen bir ödevin parçasıysa buraya bağlanıyor, bireysel ödevlerde boş kalıyor
    public int? TopluAtamaId { get; set; }
    public OdevTopluAtama? TopluAtama { get; set; }
}
