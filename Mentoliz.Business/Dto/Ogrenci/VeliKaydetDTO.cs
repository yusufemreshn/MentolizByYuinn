using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Ogrenci;

// öğrenci formunun içinde dinamik olarak eklenip çıkarılan veli satırı, yeni eklenende id sıfır kalıyor
public class VeliKaydetDTO
{
    public int Id { get; set; }

    public Yakinlik Yakinlik { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    public string Telefon { get; set; } = string.Empty;

    public string? Meslek { get; set; }

    public string? Eposta { get; set; }

    public bool BirincilIletisimMi { get; set; }
}
