using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.CalismaProgrami;

namespace Mentoliz.Business.Abstract;

public interface ICalismaProgramiServisi
{
    Task<List<CalismaProgramiDTO>> ListeleAsync(int ogrenciId);

    Task<CalismaProgramiDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<CalismaProgramiKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<CalismaProgramiDTO>> EkleAsync(CalismaProgramiKaydetDTO dto);

    Task<IslemSonucu<CalismaProgramiDTO>> GuncelleAsync(CalismaProgramiKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    // ızgaraya tek satır eklemek için, tüm haftayı yeniden göndermeye gerek kalmıyor
    Task<IslemSonucu<CalismaProgramiSatiriDTO>> SatirEkleAsync(int calismaProgramiId, CalismaProgramiSatiriKaydetDTO satir);

    Task<IslemSonucu> SatirSilAsync(int satirId);

    // ızgaradan tek tıkla tamamlandı işaretlemek için
    Task<IslemSonucu> SatirTamamlandiIsaretleAsync(int satirId, bool tamamlandiMi);

    // önceki haftanın satırlarını yeni bir haftaya kopyalar, tamamlandı işaretleri sıfırlanır
    Task<IslemSonucu<CalismaProgramiDTO>> OncekiHaftadanKopyalaAsync(int ogrenciId, DateTime yeniHaftaBaslangicTarihi);
}
