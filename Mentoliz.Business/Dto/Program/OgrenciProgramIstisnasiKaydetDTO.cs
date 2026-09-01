using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Program;

public class OgrenciProgramIstisnasiKaydetDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public Gun Gun { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    public int? DersId { get; set; }

    public ProgramIstisnaTuru Tur { get; set; }

    public string? OgretmenAdi { get; set; }

    public string Aciklama { get; set; } = string.Empty;

    public DateTime? BaslangicTarihi { get; set; }

    public DateTime? BitisTarihi { get; set; }
}
