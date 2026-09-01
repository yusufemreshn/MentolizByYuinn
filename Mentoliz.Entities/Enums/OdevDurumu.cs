namespace Mentoliz.Entities.Enums;

public enum OdevDurumu
{
    Verildi = 1,
    Tamamlandi = 2,
    EksikTamamlandi = 3,
    // bu durum elle de girilebilir ama asıl olarak okuma anında hesaplanıyor
    Gecikti = 4,
    Yapilmadi = 5
}
