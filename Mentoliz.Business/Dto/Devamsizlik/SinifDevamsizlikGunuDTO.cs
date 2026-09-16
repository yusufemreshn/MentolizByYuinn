namespace Mentoliz.Business.Dto.Devamsizlik;

// devamsızlık sayfasında bir sınıf ve tarih seçilince ekrana basılacak liste
public class SinifDevamsizlikGunuDTO
{
    public int SinifId { get; set; }

    public DateTime Tarih { get; set; }

    public List<OgrenciDevamsizlikDurumuDTO> Ogrenciler { get; set; } = [];
}
