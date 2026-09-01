using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// öğrencinin geçmiş hedefleri de saklanıyor, aynı anda sadece bir tanesi aktif oluyor
public class Hedef : BaseEntity
{
    public int OgrenciId { get; set; }
    public Ogrenci Ogrenci { get; set; } = null!;

    public string HedefUniversite { get; set; } = string.Empty;

    public string HedefBolum { get; set; } = string.Empty;

    public PuanTuru PuanTuru { get; set; }

    public int? HedefSiralama { get; set; }

    public decimal? HedefToplamNet { get; set; }

    // hedefin ne zaman oluşturulduğu zaten BaseEntity.OlusturmaTarihi alanından okunuyor, ayrıca tutmaya gerek yok
    public bool AktifMi { get; set; }

    // ders bazlı hedef netler, deneme sonuçlarıyla karşılaştırılıp hedefe uzaklık gösteriliyor
    public ICollection<HedefDersNeti> DersNetleri { get; set; } = new List<HedefDersNeti>();
}
