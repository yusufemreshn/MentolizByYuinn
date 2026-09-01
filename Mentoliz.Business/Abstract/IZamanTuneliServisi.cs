using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Abstract;

public interface IZamanTuneliServisi
{
    // görüşme, ödev, deneme sonucu ve görev kayıtlarını tek kronolojik akışta birleştirir, en yeniden eskiye sıralı döner
    Task<List<ZamanTuneliOgesiDTO>> OlusturAsync(int ogrenciId, int adet);
}
