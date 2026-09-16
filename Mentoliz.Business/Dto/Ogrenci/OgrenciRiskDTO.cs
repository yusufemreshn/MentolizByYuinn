namespace Mentoliz.Business.Dto.Ogrenci;

// erken uyarı puanı, veritabanında tutulmuyor, ana sayfa her açıldığında dört etkenden okuma anında hesaplanıyor
public class OgrenciRiskDTO
{
    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    // yüz üzerinden toplam risk puanı, yüksek olması öğretmenin daha çok dikkat etmesi gerektiği anlamına geliyor
    public int RiskPuani { get; set; }

    // kırılım şeffaf olsun diye beş alt puan da ayrı ayrı taşınıyor, hangi etken öne çıkmış görülebilsin diye
    public int NetTrendPuani { get; set; }

    public int OdevPuani { get; set; }

    public int GorusmePuani { get; set; }

    public int HedefPuani { get; set; }

    public int DevamsizlikPuani { get; set; }
}
