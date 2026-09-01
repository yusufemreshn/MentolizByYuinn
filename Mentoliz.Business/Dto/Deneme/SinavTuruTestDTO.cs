namespace Mentoliz.Business.Dto.Deneme;

public class SinavTuruTestDTO
{
    public int Id { get; set; }

    public int SinavTuruId { get; set; }

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public int SoruSayisi { get; set; }

    public int Sira { get; set; }
}
