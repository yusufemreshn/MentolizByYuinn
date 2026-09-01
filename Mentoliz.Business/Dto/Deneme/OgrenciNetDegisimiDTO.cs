namespace Mentoliz.Business.Dto.Deneme;

public class OgrenciNetDegisimiDTO
{
    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    public decimal OncekiNet { get; set; }

    public decimal SonrakiNet { get; set; }

    public decimal Degisim { get; set; }
}
