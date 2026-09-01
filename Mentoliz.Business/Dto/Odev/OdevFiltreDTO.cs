using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Odev;

public class OdevFiltreDTO
{
    public string? Arama { get; set; }

    public int? OgrenciId { get; set; }

    public int? SinifId { get; set; }

    public int? DersId { get; set; }

    public OdevDurumu? Durum { get; set; }

    // sınıf ödev yoklama matrisinde tek bir toplu atamaya bağlı ödevleri çekmek için
    public int? TopluAtamaId { get; set; }
}
