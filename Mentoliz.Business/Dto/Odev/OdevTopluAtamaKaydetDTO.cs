namespace Mentoliz.Business.Dto.Odev;

public class OdevTopluAtamaKaydetDTO
{
    public int Id { get; set; }

    public int SinifId { get; set; }

    public int DersId { get; set; }

    public string Baslik { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime SonTeslimTarihi { get; set; }
}
