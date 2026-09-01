using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Gorusme;

namespace Mentoliz.Business.Abstract;

public interface IGorusmeServisi
{
    Task<List<GorusmeDTO>> ListeleAsync(GorusmeFiltreDTO? filtre = null);

    Task<GorusmeDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<GorusmeKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    // sonraki görüşme tarihi doluysa arka planda otomatik bir görev de oluşturuluyor
    Task<IslemSonucu<GorusmeDTO>> EkleAsync(GorusmeKaydetDTO dto);

    Task<IslemSonucu<GorusmeDTO>> GuncelleAsync(GorusmeKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    // ana sayfadaki uyarı listesi için, belirlenen eşik gün sayısından uzun süredir görüşme yapılmamış aktif öğrenciler
    Task<List<UzunSureGorusulmeyenOgrenciDTO>> UzunSureGorusulmeyenleriListeleAsync();
}
