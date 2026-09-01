using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Tercih;

namespace Mentoliz.Business.Abstract;

public interface IOgrenciTercihiServisi
{
    Task<List<OgrenciTercihiDTO>> ListeleAsync(int ogrenciId);

    Task<OgrenciTercihiKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<OgrenciTercihiDTO>> EkleAsync(OgrenciTercihiKaydetDTO dto);

    Task<IslemSonucu<OgrenciTercihiDTO>> GuncelleAsync(OgrenciTercihiKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
