namespace Mentoliz.Business.Dto.Gorusme;

// görüşme hazırlık özetindeki son deneme netleri listesi için, tek bir denemenin kısa özeti
public class SonDenemeNetiDTO
{
    public string DenemeAdi { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }

    public decimal ToplamNet { get; set; }
}
