namespace Mentoliz.Web.ViewModels;

// listelerde hiç kayıt yokken gösterilen boş durum ekranının içeriğini taşıyor
public class BosDurumViewModel
{
    // bootstrap icons sınıf adı, örneğin bi-people
    public string Ikon { get; set; } = "bi-inbox";

    public string Mesaj { get; set; } = "Henüz kayıt yok.";

    // ikisi de doluysa boş durumun altında küçük bir eylem düğmesi çıkıyor, örneğin "İlk öğrenciyi ekle"
    public string? EylemBaslik { get; set; }

    public string? EylemUrl { get; set; }
}
