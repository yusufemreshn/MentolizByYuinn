using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Sinif;

namespace Mentoliz.Business.Abstract;

public interface ISinifServisi
{
    Task<List<SinifDTO>> ListeleAsync();

    Task<SinifDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için, SinifDTO'daki öğrenci sayısı gibi görüntüleme amaçlı alanları taşımaz
    Task<SinifKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<SinifDTO>> EkleAsync(SinifKaydetDTO dto);

    Task<IslemSonucu<SinifDTO>> GuncelleAsync(SinifKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
