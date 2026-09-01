namespace Mentoliz.Business.Dto.Deneme;

// sınıf bazlı ortalama net grafiğinin hangi denemeye ait olduğunu grafik başlığında gösterebilelim diye deneme bilgisini de taşıyor
public class DenemeAnaliziSonucuDTO
{
    public string SinavTuruAdi { get; set; } = string.Empty;

    public string DenemeAdi { get; set; } = string.Empty;

    public List<SinifOrtalamaNetDTO> SinifOrtalamaNetleri { get; set; } = [];
}
