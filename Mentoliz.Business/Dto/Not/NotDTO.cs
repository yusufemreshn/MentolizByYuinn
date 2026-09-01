namespace Mentoliz.Business.Dto.Not;

public class NotDTO
{
    public int Id { get; set; }

    public string Baslik { get; set; } = string.Empty;

    public string Icerik { get; set; } = string.Empty;

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime GuncellemeTarihi { get; set; }
}
