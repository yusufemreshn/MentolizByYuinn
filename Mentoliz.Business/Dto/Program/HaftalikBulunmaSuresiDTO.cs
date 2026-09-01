using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Program;

// birleşik programdan hesaplanan, o gün öğrencinin kurumda ne zamandan ne zamana kadar olacağı bilgisi
public class HaftalikBulunmaSuresiDTO
{
    public Gun Gun { get; set; }

    public TimeSpan? GirisSaati { get; set; }

    public TimeSpan? CikisSaati { get; set; }

    public TimeSpan ToplamSure { get; set; }
}
