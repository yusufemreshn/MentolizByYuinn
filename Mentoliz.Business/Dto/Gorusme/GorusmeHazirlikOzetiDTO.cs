namespace Mentoliz.Business.Dto.Gorusme;

// görüşme ekleme ekranındaki yan panelde gösterilecek, öğretmenin görüşmeye hazırlanması için hızlı özet
public class GorusmeHazirlikOzetiDTO
{
    public List<SonDenemeNetiDTO> SonDenemeNetleri { get; set; } = [];

    public int AcikOdevSayisi { get; set; }

    public string? SonAlinanKarar { get; set; }

    public DateTime? SonGorusmeTarihi { get; set; }
}
