namespace Mentoliz.Business.Dto.Deneme;

public class SinavTuruDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Kod { get; set; } = string.Empty;

    public int Sira { get; set; }

    public bool AktifMi { get; set; }

    public List<SinavTuruTestDTO> Testler { get; set; } = [];

    // sonuç girişi ekranında toplam soru sayısını göstermek için, veritabanında tutulmuyor
    public int ToplamSoruSayisi => Testler.Sum(t => t.SoruSayisi);
}
