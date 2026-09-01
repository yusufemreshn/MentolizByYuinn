namespace Mentoliz.Business.Dto.Deneme;

// radar grafiğinde bir dersin eksenini temsil ediyor
public class OgrenciDersNetiDTO
{
    public string DersAdi { get; set; } = string.Empty;

    public decimal Net { get; set; }

    public int SoruSayisi { get; set; }
}
