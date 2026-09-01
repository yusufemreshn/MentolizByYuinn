namespace Mentoliz.Business.Dto.Hedef;

// öğrenci detayındaki hedefler sekmesinde gösterilecek, veritabanında tutulmuyor
public class HedefKarsilastirmaDTO
{
    public List<DersHedefKarsilastirmaDTO> DersKarsilastirmalari { get; set; } = [];

    public decimal ToplamHedefNet { get; set; }

    // hiçbir derste sonuç girilmemişse boş kalır
    public decimal? ToplamGuncelNet { get; set; }
}
