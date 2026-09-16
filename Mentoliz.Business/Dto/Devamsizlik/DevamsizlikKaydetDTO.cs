using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Devamsizlik;

// haritadaki tek bir öğrenci satırı değiştirildiğinde gönderilen veri, konu takip haritasındaki satır kaydıyla aynı mantık
public class DevamsizlikKaydetDTO
{
    public int OgrenciId { get; set; }

    public DateTime Tarih { get; set; }

    public DevamsizlikDurumu Durum { get; set; }

    public string? Not { get; set; }
}
