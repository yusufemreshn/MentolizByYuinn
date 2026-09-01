namespace Mentoliz.Business.Dto.Deneme;

public class DenemeSonucDetayDTO
{
    public int Id { get; set; }

    public int SinavTuruTestId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public int SoruSayisi { get; set; }

    public int Dogru { get; set; }

    public int Yanlis { get; set; }

    public int Bos { get; set; }

    // veritabanında tutulmuyor, net hesaplayıcı yardımcı sınıfla okuma anında hesaplanıyor
    public decimal Net { get; set; }
}
