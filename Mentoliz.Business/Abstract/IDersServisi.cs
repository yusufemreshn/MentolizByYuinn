using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Ders;

namespace Mentoliz.Business.Abstract;

public interface IDersServisi
{
    Task<List<DersDTO>> ListeleAsync();

    Task<DersDTO?> TekGetirAsync(int id);

    Task<IslemSonucu<DersDTO>> EkleAsync(DersKaydetDTO dto);

    Task<IslemSonucu<DersDTO>> GuncelleAsync(DersKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
