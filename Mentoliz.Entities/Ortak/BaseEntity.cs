namespace Mentoliz.Entities.Ortak;

// bütün entity sınıfları buradan türeyecek, ortak alanları tek yerden yönetmek için
public abstract class BaseEntity
{
    public int Id { get; set; }

    // kayıt ilk eklendiğinde otomatik doluyor
    public DateTime OlusturmaTarihi { get; set; }

    // kayıt üzerinde her güncellemede bu tarih yenileniyor
    public DateTime GuncellemeTarihi { get; set; }

    // burası true olursa kayıt yumuşak silinmiş sayılır, veritabanından gerçekten silinmez
    public bool SilindiMi { get; set; }
}
