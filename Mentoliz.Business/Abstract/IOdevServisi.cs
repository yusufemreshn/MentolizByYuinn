using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Abstract;

public interface IOdevServisi
{
    Task<List<OdevDTO>> ListeleAsync(OdevFiltreDTO? filtre = null);

    Task<OdevDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<OdevKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<OdevDTO>> EkleAsync(OdevKaydetDTO dto);

    Task<IslemSonucu<OdevDTO>> GuncelleAsync(OdevKaydetDTO dto);

    // liste ekranından tek tıkla durum değiştirmek için, tam formu açmaya gerek kalmıyor
    Task<IslemSonucu> DurumGuncelleAsync(int id, OdevDurumu yeniDurum);

    Task<IslemSonucu> SilAsync(int id);

    Task<List<OdevTopluAtamaDTO>> TopluAtamalariListeleAsync();

    // toplu atama kaydını oluşturur ve sınıftaki her aktif öğrenci için ayrı bir odev satırı açar
    Task<IslemSonucu<OdevTopluAtamaDTO>> TopluAtamaEkleAsync(OdevTopluAtamaKaydetDTO dto);

    Task<IslemSonucu> TopluAtamaSilAsync(int id);

    // öğrenci detayındaki ödev sekmesinde gösterilecek tamamlama oranı özeti
    Task<OgrenciOdevOzetiDTO> OgrenciOzetiHesaplaAsync(int ogrenciId);

    // ana sayfada gösterilecek, henüz teslim edilmemiş ve teslim tarihi en yakın ödevler
    Task<List<OdevDTO>> YaklasanTeslimTarihleriniListeleAsync(int adet);

    // ana sayfadaki halka grafik için, bu haftanın teslim tarihi olan ödevlerin tamamlanma yüzdesi
    Task<int> HaftalikTamamlamaOraniHesaplaAsync();
}
