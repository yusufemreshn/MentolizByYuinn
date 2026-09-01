namespace Mentoliz.Business.Dto.Deneme;

public class DenemeDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }

    public int SinavTuruId { get; set; }

    public string SinavTuruAdi { get; set; } = string.Empty;

    public string? YayinAdi { get; set; }

    public string? Aciklama { get; set; }

    // veritabanında tutulmuyor, bağlı sonuç kayıtlarının sayısından hesaplanıyor
    public int KatilimciSayisi { get; set; }
}
