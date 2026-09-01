namespace Mentoliz.Business.Dto.Deneme;

// iki deneme arasında bir dersin net değişimini gösteriyor
public class DersDegisimDTO
{
    public string DersAdi { get; set; } = string.Empty;

    public decimal OncekiNet { get; set; }

    public decimal SonrakiNet { get; set; }

    public decimal Degisim { get; set; }
}
