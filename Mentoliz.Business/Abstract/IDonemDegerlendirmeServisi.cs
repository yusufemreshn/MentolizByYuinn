using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Abstract;

public interface IDonemDegerlendirmeServisi
{
    // baslangic ve bitis tarihleri dahil olacak şekilde, o aralıktaki bütün deneme, ödev ve görüşme verisini tek raporda topluyor
    Task<DonemDegerlendirmeDTO> OlusturAsync(int ogrenciId, DateTime baslangic, DateTime bitis);
}
