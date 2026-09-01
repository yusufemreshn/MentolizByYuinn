namespace Mentoliz.Business.Dto.Deneme;

// toplu sonuç girişi ızgarasında bir satırı doldurmak için, öğrenci bilgisiyle düzenlenebilir sonuç bir arada geliyor
public class DenemeSonucSatiriDTO
{
    public int OgrenciId { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;

    public DenemeSonucKaydetDTO Sonuc { get; set; } = new();
}
