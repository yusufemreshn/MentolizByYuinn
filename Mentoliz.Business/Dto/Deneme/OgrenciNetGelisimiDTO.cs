namespace Mentoliz.Business.Dto.Deneme;

// net gelişim çizgi grafiğinde bir noktayı temsil ediyor
public class OgrenciNetGelisimiDTO
{
    public int DenemeId { get; set; }

    public string DenemeAdi { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }

    public decimal ToplamNet { get; set; }
}
