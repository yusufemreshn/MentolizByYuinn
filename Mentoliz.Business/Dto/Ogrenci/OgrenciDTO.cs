using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Ogrenci;

// listeleme ve detay ekranında kullanılacak, entity hiçbir zaman view'a gitmiyor
public class OgrenciDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    // listelerde tek sütunda göstermek kolay olsun diye hazır birleştirilmiş halini de tutuyoruz
    public string AdSoyad => $"{Ad} {Soyad}";

    public string? Telefon { get; set; }

    public string? OgrenciNo { get; set; }

    public int? SinifId { get; set; }

    // sınıf adı burada hazır geliyor ki view sınıf entity'sine hiç dokunmasın
    public string? SinifAdi { get; set; }

    public DateTime? DogumTarihi { get; set; }

    public Cinsiyet Cinsiyet { get; set; }

    public string? OkulAdi { get; set; }

    public Alan Alan { get; set; }

    public DateTime KayitTarihi { get; set; }

    public bool AktifMi { get; set; }

    public string? FotografYolu { get; set; }

    public string? GenelNotlar { get; set; }

    public List<VeliDTO> Veliler { get; set; } = [];
}
