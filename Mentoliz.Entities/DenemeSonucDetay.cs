using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// bir denemenin bir testine ait doğru yanlış boş sayıları, net burada tutulmuyor okuma anında hesaplanıyor
public class DenemeSonucDetay : BaseEntity
{
    public int DenemeSonucId { get; set; }
    public DenemeSonuc DenemeSonuc { get; set; } = null!;

    public int SinavTuruTestId { get; set; }
    public SinavTuruTest SinavTuruTest { get; set; } = null!;

    public int Dogru { get; set; }

    public int Yanlis { get; set; }

    public int Bos { get; set; }
}
