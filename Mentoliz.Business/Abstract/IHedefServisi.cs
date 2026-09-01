using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Hedef;

namespace Mentoliz.Business.Abstract;

public interface IHedefServisi
{
    Task<List<HedefDTO>> ListeleAsync(int ogrenciId);

    Task<HedefDTO?> AktifHedefGetirAsync(int ogrenciId);

    Task<HedefDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<HedefKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    // yeni hedef eklenince öğrencinin önceki aktif hedefi otomatik pasife düşüyor
    Task<IslemSonucu<HedefDTO>> EkleAsync(HedefKaydetDTO dto);

    Task<IslemSonucu<HedefDTO>> GuncelleAsync(HedefKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    // aktif hedefteki ders bazlı netleri, öğrencinin en son deneme sonuçlarıyla karşılaştırır, aktif hedef yoksa null döner
    Task<HedefKarsilastirmaDTO?> HedefeUzaklikHesaplaAsync(int ogrenciId);
}
