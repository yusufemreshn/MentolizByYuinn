using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

public class HedefDersNeti : BaseEntity
{
    public int HedefId { get; set; }
    public Hedef Hedef { get; set; } = null!;

    public int DersId { get; set; }
    public Ders Ders { get; set; } = null!;

    public decimal HedefNet { get; set; }
}
