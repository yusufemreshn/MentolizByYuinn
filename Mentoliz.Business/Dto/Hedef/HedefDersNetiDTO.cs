namespace Mentoliz.Business.Dto.Hedef;

public class HedefDersNetiDTO
{
    public int Id { get; set; }

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public decimal HedefNet { get; set; }
}
