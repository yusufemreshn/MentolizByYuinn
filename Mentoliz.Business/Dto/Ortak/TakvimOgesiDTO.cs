namespace Mentoliz.Business.Dto.Ortak;

// takvim ekranında bir güne düşen tek bir olayı temsil ediyor, görüşme/ödev/deneme/görev kayıtlarının ortak biçimi
public class TakvimOgesiDTO
{
    public DateTime Tarih { get; set; }

    // "gorusme", "odev", "deneme", "gorev" değerlerinden biri, ikon ve link seçimi view tarafında bu alana göre yapılıyor
    public string Tur { get; set; } = string.Empty;

    public string Baslik { get; set; } = string.Empty;

    // kaynak kaydın kendi id'si, tıklanınca ilgili detay sayfasına gitmek için
    public int IlgiliId { get; set; }
}
