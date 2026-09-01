using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Program;

namespace Mentoliz.Business.Abstract;

public interface IOgrenciProgramServisi
{
    Task<List<OgrenciProgramIstisnasiDTO>> IstisnalariListeleAsync(int ogrenciId);

    // düzenleme formunu doldurmak için
    Task<OgrenciProgramIstisnasiKaydetDTO?> IstisnaDuzenlemeIcinGetirAsync(int id);

    // sınıf ders programı şablonu ile öğrencinin geçerli istisnalarını birleştirip haftalık programı çıkarır
    Task<List<BirlesikProgramSatiriDTO>> BirlesikProgramiHesaplaAsync(int ogrenciId);

    // birleşik programdan her gün için giriş, çıkış ve toplam süreyi hesaplar
    Task<List<HaftalikBulunmaSuresiDTO>> HaftalikBulunmaSuresiHesaplaAsync(int ogrenciId);

    Task<IslemSonucu<OgrenciProgramIstisnasiDTO>> IstisnaEkleAsync(OgrenciProgramIstisnasiKaydetDTO dto);

    Task<IslemSonucu<OgrenciProgramIstisnasiDTO>> IstisnaGuncelleAsync(OgrenciProgramIstisnasiKaydetDTO dto);

    Task<IslemSonucu> IstisnaSilAsync(int id);
}
