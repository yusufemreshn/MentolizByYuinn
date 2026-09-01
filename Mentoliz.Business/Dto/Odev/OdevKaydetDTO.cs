using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Odev;

public class OdevKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public int DersId { get; set; }

    public string KonuBasligi { get; set; } = string.Empty;

    public string? KaynakAdi { get; set; }

    public int? ToplamSoruSayisi { get; set; }

    public int? TamamlananSoruSayisi { get; set; }

    public DateTime VerilisTarihi { get; set; }

    public DateTime SonTeslimTarihi { get; set; }

    public DateTime? YapilmaTarihi { get; set; }

    public OdevDurumu Durum { get; set; }

    public string? Aciklama { get; set; }
}
