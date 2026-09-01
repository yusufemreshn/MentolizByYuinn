using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Dto.Ogrenci;

// hem ekleme hem güncelleme formunda kullanılıyor, yeni kayıtta id sıfır geliyor
public class OgrenciKaydetDTO
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    public string? Telefon { get; set; }

    public string? OgrenciNo { get; set; }

    public int? SinifId { get; set; }

    public DateTime? DogumTarihi { get; set; }

    public Cinsiyet Cinsiyet { get; set; }

    public string? OkulAdi { get; set; }

    public Alan Alan { get; set; }

    public DateTime KayitTarihi { get; set; }

    public bool AktifMi { get; set; }

    public string? FotografYolu { get; set; }

    public string? GenelNotlar { get; set; }

    // form üzerinde dinamik eklenip çıkarılan veliler, boş liste de olabilir
    public List<VeliKaydetDTO> Veliler { get; set; } = [];
}
