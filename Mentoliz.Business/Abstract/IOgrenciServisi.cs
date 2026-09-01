using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Abstract;

public interface IOgrenciServisi
{
    Task<List<OgrenciDTO>> ListeleAsync(OgrenciFiltreDTO? filtre = null);

    Task<OgrenciDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için, OgrenciDTO'daki görüntüleme amaçlı alanları taşımaz
    Task<OgrenciKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<OgrenciDTO>> EkleAsync(OgrenciKaydetDTO dto);

    Task<IslemSonucu<OgrenciDTO>> GuncelleAsync(OgrenciKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);
}
