using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Gorev;

namespace Mentoliz.Business.Abstract;

public interface IGorevServisi
{
    Task<List<GorevDTO>> ListeleAsync(GorevFiltreDTO? filtre = null);

    Task<GorevDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<GorevKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<GorevDTO>> EkleAsync(GorevKaydetDTO dto);

    Task<IslemSonucu<GorevDTO>> GuncelleAsync(GorevKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    Task<IslemSonucu> TamamlandiIsaretleAsync(int id);

    // ana sayfada bugün görüşülecek öğrencileri göstermek için, öğrenciyle ilişkili ve henüz tamamlanmamış görevler
    Task<List<GorevDTO>> BugunkuGorevleriListeleAsync();
}
