namespace Mentoliz.Web.ViewModels;

// hedef formundaki ders bazlı net girişi satırı, boş bırakılan dersler kaydedilmiyor
public class HedefDersNetiSatiriViewModel
{
    public int Id { get; set; }

    public int DersId { get; set; }

    public string DersAdi { get; set; } = string.Empty;

    public decimal? HedefNet { get; set; }
}
