using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// rehber öğretmenin kendi kullanımı için tuttuğu serbest notlar, öğrenciyle bağlantısı yok
public class Not : BaseEntity
{
    public string Baslik { get; set; } = string.Empty;

    public string Icerik { get; set; } = string.Empty;
}
