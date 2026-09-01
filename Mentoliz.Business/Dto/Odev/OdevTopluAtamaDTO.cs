namespace Mentoliz.Business.Dto.Odev;

public class OdevTopluAtamaDTO
{
    public int Id { get; set; }

    public int SinifId { get; set; }

    public string SinifAdi { get; set; } = string.Empty;

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public string Baslik { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime SonTeslimTarihi { get; set; }

    // veritabanında tutulmuyor, bağlı odev kayıtlarının sayısından hesaplanıyor
    public int OdevSayisi { get; set; }
}
