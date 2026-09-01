using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// bir sınav türünde hangi dersten kaç soru sorulduğunu tanımlıyor, örneğin tyt türkçe kırk soru
public class SinavTuruTest : BaseEntity
{
    public int SinavTuruId { get; set; }
    public SinavTuru SinavTuru { get; set; } = null!;

    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public int SoruSayisi { get; set; }

    // sonuç girişi ekranında testler bu sıraya göre listelenecek
    public int Sira { get; set; }

    public ICollection<DenemeSonucDetay> SonucDetaylari { get; set; } = new List<DenemeSonucDetay>();
}
