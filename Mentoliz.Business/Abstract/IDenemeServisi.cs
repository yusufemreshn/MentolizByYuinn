using Mentoliz.Business.Helpers;
using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Business.Abstract;

public interface IDenemeServisi
{
    Task<List<DenemeDTO>> ListeleAsync();

    Task<DenemeDTO?> TekGetirAsync(int id);

    // düzenleme formunu doldurmak için
    Task<DenemeKaydetDTO?> DuzenlemeIcinGetirAsync(int id);

    Task<IslemSonucu<DenemeDTO>> EkleAsync(DenemeKaydetDTO dto);

    Task<IslemSonucu<DenemeDTO>> GuncelleAsync(DenemeKaydetDTO dto);

    Task<IslemSonucu> SilAsync(int id);

    Task<List<DenemeSonucDTO>> SonuclariListeleAsync(int denemeId);

    Task<DenemeSonucDTO?> SonucGetirAsync(int id);

    // id sıfırsa yeni sonuç, doluysa mevcut sonucun güncellenmesi olarak çalışıyor
    Task<IslemSonucu<DenemeSonucDTO>> SonucKaydetAsync(DenemeSonucKaydetDTO dto);

    Task<IslemSonucu> SonucSilAsync(int id);

    // toplu sonuç girişi ızgarasını doldurmak için, sınıftaki her öğrenci için var olan sonucu veya boş şablonu hazırlar
    Task<List<DenemeSonucSatiriDTO>> SonucGirisiSatirlariniHazirlaAsync(int denemeId, int sinifId);

    // önceki denemeden doldurma düğmesi için, aynı sınav türünden daha önceki en güncel denemedeki sonuçlar
    Task<List<OgrenciOncekiSonucDTO>> OncekiDenemeSonuclariniGetirAsync(int denemeId, int sinifId);

    // ana sayfa istatistiği için, bu hafta içinde sisteme kaydedilmiş deneme sayısı
    Task<int> BuHaftaGirilenSayisiniHesaplaAsync();
}
