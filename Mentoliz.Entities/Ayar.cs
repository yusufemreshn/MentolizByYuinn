using Mentoliz.Entities.Ortak;

namespace Mentoliz.Entities;

// yedekleme klasörü yolu, kurum adı, öğretmen adı gibi tekil ayarlar için basit anahtar değer tablosu
public class Ayar : BaseEntity
{
    public string Anahtar { get; set; } = string.Empty;

    public string Deger { get; set; } = string.Empty;
}
