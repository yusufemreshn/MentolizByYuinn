using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Devamsizlik;

// sınıf bazlı toplu ekranda tek bir öğrenci satırını temsil ediyor
public class OgrenciDevamsizlikDurumuDTO
{
    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    // o gün için kayıt yoksa Geldi olarak gelir
    public DevamsizlikDurumu Durum { get; set; }

    public string? Not { get; set; }
}
