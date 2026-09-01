namespace Mentoliz.Business.Dto.Deneme;

// öğrenci detayındaki deneme sonuçları sekmesinde, seçilen sınav türü için gösterilecek tüm analiz bir arada
public class OgrenciDenemeAnaliziDTO
{
    public List<OgrenciNetGelisimiDTO> NetGelisimi { get; set; } = [];

    // en son katıldığı denemenin ders bazlı netleri, yatay çubuk grafiği için
    public List<OgrenciDersNetiDTO> SonDenemeDersNetleri { get; set; } = [];

    public string? SonDenemeAdi { get; set; }

    public decimal? SonDenemeToplamNet { get; set; }

    // aynı denemeye giren sınıf arkadaşlarının ortalaması, karşılaştırma için
    public decimal? SinifOrtalamasi { get; set; }

    // son denemede tüm katılımcılar arasındaki net'e göre sıra
    public int? KurumSiralamasi { get; set; }

    public int? ToplamKatilimci { get; set; }

    // son iki deneme arasında en çok net kazandıran ve kaybettiren dersler
    public List<DersDegisimDTO> EnCokYukselenler { get; set; } = [];

    public List<DersDegisimDTO> EnCokDusenler { get; set; } = [];

    // net/soru sayısı oranı en düşük olan dersler, zayıf alan olarak işaretleniyor
    public List<string> ZayifAlanlar { get; set; } = [];
}
