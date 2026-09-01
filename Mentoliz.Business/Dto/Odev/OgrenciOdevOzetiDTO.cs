namespace Mentoliz.Business.Dto.Odev;

// öğrenci detayındaki ödev sekmesinde gösterilecek, veritabanında tutulmuyor
public class OgrenciOdevOzetiDTO
{
    public int ToplamOdevSayisi { get; set; }

    public int TamamlananOdevSayisi { get; set; }

    public int GecikenOdevSayisi { get; set; }

    // yüzde olarak, toplam ödev yoksa sıfır kabul ediliyor
    public int TamamlamaOrani { get; set; }
}
