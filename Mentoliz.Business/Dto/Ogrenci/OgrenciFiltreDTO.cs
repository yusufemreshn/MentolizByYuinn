namespace Mentoliz.Business.Dto.Ogrenci;

// listeleme sayfasındaki arama kutusu ve filtre alanlarını taşıyor
public class OgrenciFiltreDTO
{
    // ad, soyad veya öğrenci numarasında aranıyor
    public string? Arama { get; set; }

    public int? SinifId { get; set; }

    public bool? AktifMi { get; set; }
}
