namespace Mentoliz.Business.Dto.Deneme;

public class DenemeKaydetDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }

    public int SinavTuruId { get; set; }

    public string? YayinAdi { get; set; }

    public string? Aciklama { get; set; }
}
