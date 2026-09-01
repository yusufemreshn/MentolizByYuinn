using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Business.Abstract;

public interface IRiskDegerlendirmeServisi
{
    // aktif öğrencilerin risk puanlarını hesaplar, en yüksek riskliden başlayarak sıralı döner
    Task<List<OgrenciRiskDTO>> DegerlendirAsync();

    // yüksek riskli öğrenciler için görüşme planlama görevi açar, aynı öğrenci için açık bir risk görevi zaten varsa tekrar oluşturmaz
    Task OtomatikGorevOlusturAsync(List<OgrenciRiskDTO> riskListesi);
}
