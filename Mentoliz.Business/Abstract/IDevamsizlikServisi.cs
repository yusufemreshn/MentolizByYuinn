using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Business.Helpers;

namespace Mentoliz.Business.Abstract;

public interface IDevamsizlikServisi
{
    // bir sınıfın belirli bir gündeki tüm öğrencilerinin devamsızlık durumunu döner, kaydı olmayanlar geldi görünür
    Task<SinifDevamsizlikGunuDTO> SinifGunlukDurumGetirAsync(int sinifId, DateTime tarih);

    // haritadaki tek bir öğrenci satırı değiştiğinde çağrılıyor, geldi seçilirse varsa eski kayıt yumuşak siliniyor
    Task<IslemSonucu> DurumKaydetAsync(DevamsizlikKaydetDTO dto);

    // tek öğrencinin son otuz günlük özeti, öğrenci detay sayfasında gösteriliyor
    Task<OgrenciDevamsizlikOzetiDTO> OgrenciOzetiHesaplaAsync(int ogrenciId);

    // risk değerlendirmesi bütün aktif öğrencilerin özetini tek seferde çekiyor, öğrenci başına ayrı sorgu atmasın diye
    Task<List<OgrenciDevamsizlikOzetiDTO>> TumOgrencilerOzetiHesaplaAsync();
}
