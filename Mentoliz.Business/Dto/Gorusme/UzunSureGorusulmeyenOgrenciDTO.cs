namespace Mentoliz.Business.Dto.Gorusme;

public class UzunSureGorusulmeyenOgrenciDTO
{
    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    // hiç görüşme yapılmamışsa boş kalır
    public DateTime? SonGorusmeTarihi { get; set; }

    public int? GecenGunSayisi { get; set; }
}
