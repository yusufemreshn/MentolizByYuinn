using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Business.Abstract;

public interface IDersProgramiServisi
{
    Task<List<DersProgramiSatiriDTO>> ListeleAsync(int sinifId);

    // düzenleme formunu doldurmak için
    Task<DersProgramiSatiriKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<DersProgramiSatiriDTO>> EkleAsync(DersProgramiSatiriKaydetDTO dto);

    Task<IslemSonucu<DersProgramiSatiriDTO>> GuncelleAsync(DersProgramiSatiriKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
