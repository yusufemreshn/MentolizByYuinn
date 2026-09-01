using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Konu;

namespace Mentoliz.Business.Abstract;

public interface IKonuServisi
{
    Task<List<KonuDTO>> ListeleAsync(int? dersId = null);

    Task<KonuDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<KonuKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<KonuDTO>> EkleAsync(KonuKaydetDTO dto);

    Task<IslemSonucu<KonuDTO>> GuncelleAsync(KonuKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    Task<List<OgrenciKonuTakipDTO>> TakipListeleAsync(int ogrenciId);

    // ısı haritası görünümü için tüm aktif konuları, öğrencinin takip durumuyla (yoksa başlanmadı varsayılanıyla) birleştirir
    Task<List<OgrenciKonuTakipDTO>> TumKonularIcinTakipDurumlariniGetirAsync(int ogrenciId);

    // öğrenci konu çifti için kayıt yoksa oluşturuyor, varsa durumunu güncelliyor
    Task<IslemSonucu<OgrenciKonuTakipDTO>> DurumIsaretleAsync(OgrenciKonuTakipKaydetDTO dto);

    // konu takip sekmesindeki ders bazlı ilerleme çubukları için
    Task<List<DersIlerlemeDTO>> DersBazliIlerlemeHesaplaAsync(int ogrenciId);

    // deneme analizindeki zayıf derslere ait, henüz tamamlanmamış veya tekrar gereken konuları önerir
    Task<List<KonuDTO>> ZayifKonuOnerileriHesaplaAsync(int ogrenciId);
}
