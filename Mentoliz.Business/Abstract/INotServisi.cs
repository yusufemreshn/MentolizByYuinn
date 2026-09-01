using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Not;

namespace Mentoliz.Business.Abstract;

public interface INotServisi
{
    Task<List<NotDTO>> ListeleAsync();

    // düzenleme formunu doldurmak için
    Task<NotKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<NotDTO>> EkleAsync(NotKaydetDTO dto);

    Task<IslemSonucu<NotDTO>> GuncelleAsync(NotKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
