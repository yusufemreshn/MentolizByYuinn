using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class DenemeServisi : IDenemeServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DenemeKaydetDTO> _denemeDogrulayici;
    private readonly IValidator<DenemeSonucKaydetDTO> _sonucDogrulayici;

    public DenemeServisi(
        IUnitOfWork unitOfWork,
        IValidator<DenemeKaydetDTO> denemeDogrulayici,
        IValidator<DenemeSonucKaydetDTO> sonucDogrulayici)
    {
        _unitOfWork = unitOfWork;
        _denemeDogrulayici = denemeDogrulayici;
        _sonucDogrulayici = sonucDogrulayici;
    }

    public async Task<List<DenemeDTO>> ListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Deneme>();

        var denemeler = await depo.Sorgu
            .Include(d => d.SinavTuru)
            .Include(d => d.Sonuclar)
            .OrderByDescending(d => d.Tarih)
            .ToListAsync();

        return denemeler.Select(d => d.Dto()).ToList();
    }

    public async Task<DenemeDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Deneme>();

        var deneme = await depo.Sorgu
            .Include(d => d.SinavTuru)
            .Include(d => d.Sonuclar)
            .FirstOrDefaultAsync(d => d.Id == id);

        return deneme?.Dto();
    }

    public async Task<DenemeKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Deneme>();
        var deneme = await depo.TekGetirAsync(id);

        return deneme?.KaydetDto();
    }

    public async Task<IslemSonucu<DenemeDTO>> EkleAsync(DenemeKaydetDTO dto)
    {
        var dogrulamaSonucu = await _denemeDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DenemeDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Deneme>();
        var deneme = dto.Entity();

        await depo.EkleAsync(deneme);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DenemeDTO>.Basar(deneme.Dto());
    }

    public async Task<IslemSonucu<DenemeDTO>> GuncelleAsync(DenemeKaydetDTO dto)
    {
        var dogrulamaSonucu = await _denemeDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DenemeDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Deneme>();
        var mevcutDeneme = await depo.TekGetirAsync(dto.Id);
        if (mevcutDeneme is null)
        {
            return IslemSonucu<DenemeDTO>.Basarisiz("Güncellenecek deneme bulunamadı.");
        }

        dto.Uygula(mevcutDeneme);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DenemeDTO>.Basar(mevcutDeneme.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Deneme>();
        var deneme = await depo.TekGetirAsync(id);
        if (deneme is null)
        {
            return IslemSonucu.Basarisiz("Silinecek deneme bulunamadı.");
        }

        depo.SilmeyeIsaretle(deneme);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<DenemeSonucDTO>> SonuclariListeleAsync(int denemeId)
    {
        var depo = _unitOfWork.RepositoryGetir<DenemeSonuc>();

        var sonuclar = await depo.Sorgu
            .Where(s => s.DenemeId == denemeId)
            .Include(s => s.Ogrenci)
            .Include(s => s.Detaylar).ThenInclude(d => d.SinavTuruTest).ThenInclude(t => t.Ders)
            .ToListAsync();

        return sonuclar.Select(s => s.Dto()).ToList();
    }

    public async Task<DenemeSonucDTO?> SonucGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<DenemeSonuc>();

        var sonuc = await depo.Sorgu
            .Include(s => s.Ogrenci)
            .Include(s => s.Detaylar).ThenInclude(d => d.SinavTuruTest).ThenInclude(t => t.Ders)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sonuc?.Dto();
    }

    public async Task<IslemSonucu<DenemeSonucDTO>> SonucKaydetAsync(DenemeSonucKaydetDTO dto)
    {
        var dogrulamaSonucu = await _sonucDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DenemeSonucDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<DenemeSonuc>();

        DenemeSonuc sonuc;
        if (dto.Id == 0)
        {
            sonuc = dto.Entity();
            foreach (var detayDto in dto.Detaylar)
            {
                sonuc.Detaylar.Add(detayDto.Entity());
            }

            await depo.EkleAsync(sonuc);
        }
        else
        {
            var mevcutSonuc = await depo.Sorgu
                .Include(s => s.Detaylar)
                .FirstOrDefaultAsync(s => s.Id == dto.Id);

            if (mevcutSonuc is null)
            {
                return IslemSonucu<DenemeSonucDTO>.Basarisiz("Güncellenecek deneme sonucu bulunamadı.");
            }

            dto.Uygula(mevcutSonuc);
            DetaylariEslestir(dto.Detaylar, mevcutSonuc);
            sonuc = mevcutSonuc;
        }

        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DenemeSonucDTO>.Basar(sonuc.Dto());
    }

    public async Task<IslemSonucu> SonucSilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var sonuc = await depo.TekGetirAsync(id);
        if (sonuc is null)
        {
            return IslemSonucu.Basarisiz("Silinecek deneme sonucu bulunamadı.");
        }

        depo.SilmeyeIsaretle(sonuc);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<DenemeSonucSatiriDTO>> SonucGirisiSatirlariniHazirlaAsync(int denemeId, int sinifId)
    {
        var denemeDepo = _unitOfWork.RepositoryGetir<Deneme>();
        var deneme = await denemeDepo.Sorgu
            .Include(d => d.SinavTuru).ThenInclude(s => s.Testler)
            .FirstOrDefaultAsync(d => d.Id == denemeId);

        if (deneme is null)
        {
            return [];
        }

        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var ogrenciler = await ogrenciDepo.ListeleAsync(o => o.SinifId == sinifId && o.AktifMi);

        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var mevcutSonuclar = await sonucDepo.Sorgu
            .Where(s => s.DenemeId == denemeId)
            .Include(s => s.Detaylar)
            .ToListAsync();

        var satirlar = new List<DenemeSonucSatiriDTO>();
        foreach (var ogrenci in ogrenciler.OrderBy(o => o.Ad).ThenBy(o => o.Soyad))
        {
            var mevcutSonuc = mevcutSonuclar.FirstOrDefault(s => s.OgrenciId == ogrenci.Id);

            var sonucDto = mevcutSonuc is not null
                ? mevcutSonuc.KaydetDto()
                : new DenemeSonucKaydetDTO
                {
                    DenemeId = denemeId,
                    OgrenciId = ogrenci.Id,
                    Detaylar = deneme.SinavTuru.Testler
                        .OrderBy(t => t.Sira)
                        .Select(t => new DenemeSonucDetayKaydetDTO { SinavTuruTestId = t.Id })
                        .ToList()
                };

            satirlar.Add(new DenemeSonucSatiriDTO
            {
                OgrenciId = ogrenci.Id,
                OgrenciAdSoyad = $"{ogrenci.Ad} {ogrenci.Soyad}",
                Sonuc = sonucDto
            });
        }

        return satirlar;
    }

    public async Task<List<OgrenciOncekiSonucDTO>> OncekiDenemeSonuclariniGetirAsync(int denemeId, int sinifId)
    {
        var denemeDepo = _unitOfWork.RepositoryGetir<Deneme>();
        var deneme = await denemeDepo.TekGetirAsync(denemeId);
        if (deneme is null)
        {
            return [];
        }

        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var ogrenciIdleri = (await ogrenciDepo.ListeleAsync(o => o.SinifId == sinifId && o.AktifMi))
            .Select(o => o.Id)
            .ToHashSet();

        // sınav türü testleri denemeler arasında paylaşıldığı için sinavturutestid eşleşmesi ders eşleşmesiyle aynı işi görüyor
        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var oncekiSonuclar = await sonucDepo.Sorgu
            .Where(s => s.Deneme.SinavTuruId == deneme.SinavTuruId && s.Deneme.Tarih < deneme.Tarih && !s.KatilmadiMi && ogrenciIdleri.Contains(s.OgrenciId))
            .Include(s => s.Detaylar)
            .OrderByDescending(s => s.Deneme.Tarih)
            .ToListAsync();

        var sonuc = new List<OgrenciOncekiSonucDTO>();
        foreach (var ogrenciId in ogrenciIdleri)
        {
            // sıralama tarihe göre azalan olduğu için bir öğrenci için bulunan ilk kayıt onun en güncel önceki sonucu oluyor
            var enSonOncekiSonuc = oncekiSonuclar.FirstOrDefault(s => s.OgrenciId == ogrenciId);
            if (enSonOncekiSonuc is null)
            {
                continue;
            }

            sonuc.Add(new OgrenciOncekiSonucDTO
            {
                OgrenciId = ogrenciId,
                Detaylar = enSonOncekiSonuc.Detaylar
                    .Select(d => new DenemeSonucDetayKaydetDTO { SinavTuruTestId = d.SinavTuruTestId, Dogru = d.Dogru, Yanlis = d.Yanlis, Bos = d.Bos })
                    .ToList()
            });
        }

        return sonuc;
    }

    public async Task<int> BuHaftaGirilenSayisiniHesaplaAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Deneme>();
        var haftaBaslangici = HaftaYardimcisi.HaftaBaslangici(DateTime.Today);
        var haftaBitisi = HaftaYardimcisi.HaftaBitisi(haftaBaslangici);

        return await depo.Sorgu.CountAsync(d => d.OlusturmaTarihi >= haftaBaslangici && d.OlusturmaTarihi < haftaBitisi);
    }

    // formdan gelen test detaylarını mevcut kayıtlarla karşılaştırıp ekleme, güncelleme ve silme uyguluyor
    private void DetaylariEslestir(List<DenemeSonucDetayKaydetDTO> gelenDetaylar, DenemeSonuc mevcutSonuc)
    {
        var detayDepo = _unitOfWork.RepositoryGetir<DenemeSonucDetay>();

        var gelenIdler = gelenDetaylar.Where(d => d.Id != 0).Select(d => d.Id).ToHashSet();
        foreach (var kaldirilanDetay in mevcutSonuc.Detaylar.Where(d => !gelenIdler.Contains(d.Id)).ToList())
        {
            detayDepo.SilmeyeIsaretle(kaldirilanDetay);
        }

        foreach (var detayDto in gelenDetaylar)
        {
            if (detayDto.Id == 0)
            {
                mevcutSonuc.Detaylar.Add(detayDto.Entity());
                continue;
            }

            var mevcutDetay = mevcutSonuc.Detaylar.FirstOrDefault(d => d.Id == detayDto.Id);
            if (mevcutDetay is not null)
            {
                detayDto.Uygula(mevcutDetay);
            }
        }
    }
}
