using Mentoliz.Business.Dto.Gorusme;

namespace Mentoliz.Business.Abstract;

public interface IGorusmeHazirlikServisi
{
    // görüşme ekleme ekranında öğrenci seçilince yan panelde gösterilecek hızlı özeti hazırlar
    Task<GorusmeHazirlikOzetiDTO> OzetGetirAsync(int ogrenciId);
}
