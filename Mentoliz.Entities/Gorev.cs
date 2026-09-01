using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// öğretmenin kendi yapılacaklar listesi, bazı görevler sistem tarafından otomatik de oluşturulabiliyor, örneğin görüşmede sonraki tarih girildiğinde
public class Gorev : BaseEntity
{
    public string Baslik { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    // öğrenciyle ilgisi olmayan genel görevler de olabiliyor, o yüzden opsiyonel
    public int? OgrenciId { get; set; }
    public Ogrenci? Ogrenci { get; set; }

    public GorevOnceligi Oncelik { get; set; }

    public bool TamamlandiMi { get; set; }
}
