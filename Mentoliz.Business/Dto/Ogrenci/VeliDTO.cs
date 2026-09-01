using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Ogrenci;

// listeleme ve detay ekranında gösterilecek veli bilgisi
public class VeliDTO
{
    public int Id { get; set; }

    public int OgrenciId { get; set; }

    public Yakinlik Yakinlik { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    public string Telefon { get; set; } = string.Empty;

    public string? Meslek { get; set; }

    public string? Eposta { get; set; }

    public bool BirincilIletisimMi { get; set; }
}
