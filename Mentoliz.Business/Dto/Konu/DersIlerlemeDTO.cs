namespace Mentoliz.Business.Dto.Konu;

// öğrenci detayındaki konu takip sekmesinde, ders bazlı ilerleme çubuğu için
public class DersIlerlemeDTO
{
    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public int ToplamKonuSayisi { get; set; }

    public int TamamlananKonuSayisi { get; set; }

    public int IlerlemeYuzdesi { get; set; }
}
