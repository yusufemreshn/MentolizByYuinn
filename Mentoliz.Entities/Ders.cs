using Mentoliz.Entities.Enums;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class Ders : BaseEntity
{
    public string Ad { get; set; } = string.Empty;

    // listelerde ve tablo başlıklarında yer kaplamasın diye kısa ad tutuyoruz
    public string KisaAd { get; set; } = string.Empty;

    public DersKategori Kategori { get; set; }

    // listelerde hangi sırada görünecek
    public int Sira { get; set; }

    public bool AktifMi { get; set; }

    public ICollection<DersProgramiSatiri> DersProgramiSatirlari { get; set; } = new List<DersProgramiSatiri>();

    public ICollection<OgrenciProgramIstisnasi> ProgramIstisnalari { get; set; } = new List<OgrenciProgramIstisnasi>();

    public ICollection<Odev> Odevler { get; set; } = new List<Odev>();

    public ICollection<OdevTopluAtama> OdevTopluAtamalari { get; set; } = new List<OdevTopluAtama>();

    public ICollection<SinavTuruTest> SinavTuruTestleri { get; set; } = new List<SinavTuruTest>();

    public ICollection<CalismaProgramiSatiri> CalismaProgramiSatirlari { get; set; } = new List<CalismaProgramiSatiri>();

    public ICollection<Konu> Konular { get; set; } = new List<Konu>();

    public ICollection<HedefDersNeti> HedefDersNetleri { get; set; } = new List<HedefDersNeti>();
}
