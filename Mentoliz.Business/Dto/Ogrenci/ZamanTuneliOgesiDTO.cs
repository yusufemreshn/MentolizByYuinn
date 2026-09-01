namespace Mentoliz.Business.Dto.Ogrenci;

// öğrenci detayındaki zaman tüneli sekmesinde gösterilecek, farklı kayıt tiplerinin ortak biçimi
public class ZamanTuneliOgesiDTO
{
    public DateTime Tarih { get; set; }

    // "gorusme", "odev", "deneme", "gorev" değerlerinden biri, ikon ve renk seçimi view tarafında bu alana göre yapılıyor
    public string Tur { get; set; } = string.Empty;

    public string Baslik { get; set; } = string.Empty;

    public string? Aciklama { get; set; }
}
