using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Hedef;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class HedefServisi : IHedefServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<HedefKaydetDTO> _dogrulayici;

    public HedefServisi(IUnitOfWork unitOfWork, IValidator<HedefKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<HedefDTO>> ListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        var hedefler = await depo.Sorgu
            .Where(h => h.OgrenciId == ogrenciId)
            .Include(h => h.DersNetleri).ThenInclude(d => d.Ders)
            .OrderByDescending(h => h.OlusturmaTarihi)
            .ToListAsync();

        return hedefler.Select(h => h.Dto()).ToList();
    }

    public async Task<HedefDTO?> AktifHedefGetirAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        var hedef = await depo.Sorgu
            .Include(h => h.DersNetleri).ThenInclude(d => d.Ders)
            .FirstOrDefaultAsync(h => h.OgrenciId == ogrenciId && h.AktifMi);

        return hedef?.Dto();
    }

    public async Task<HedefDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        var hedef = await depo.Sorgu
            .Include(h => h.DersNetleri).ThenInclude(d => d.Ders)
            .FirstOrDefaultAsync(h => h.Id == id);

        return hedef?.Dto();
    }

    public async Task<HedefKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        var hedef = await depo.Sorgu
            .Include(h => h.DersNetleri)
            .FirstOrDefaultAsync(h => h.Id == id);

        return hedef?.KaydetDto();
    }

    public async Task<IslemSonucu<HedefDTO>> EkleAsync(HedefKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<HedefDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        // aynı anda yalnızca bir hedef aktif olabiliyor, öncekileri pasife çekiyoruz
        var oncekiAktifHedefler = await depo.ListeleAsync(h => h.OgrenciId == dto.OgrenciId && h.AktifMi);
        foreach (var oncekiHedef in oncekiAktifHedefler)
        {
            oncekiHedef.AktifMi = false;
            depo.Guncelle(oncekiHedef);
        }

        var hedef = dto.Entity();
        hedef.AktifMi = true;
        foreach (var dersNetiDto in dto.DersNetleri)
        {
            hedef.DersNetleri.Add(dersNetiDto.Entity());
        }

        await depo.EkleAsync(hedef);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<HedefDTO>.Basar(hedef.Dto());
    }

    public async Task<IslemSonucu<HedefDTO>> GuncelleAsync(HedefKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<HedefDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Hedef>();

        var mevcutHedef = await depo.Sorgu
            .Include(h => h.DersNetleri)
            .FirstOrDefaultAsync(h => h.Id == dto.Id);

        if (mevcutHedef is null)
        {
            return IslemSonucu<HedefDTO>.Basarisiz("Güncellenecek hedef bulunamadı.");
        }

        dto.Uygula(mevcutHedef);
        DersNetleriniEslestir(dto.DersNetleri, mevcutHedef);

        await _unitOfWork.KaydetAsync();

        return IslemSonucu<HedefDTO>.Basar(mevcutHedef.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Hedef>();
        var hedef = await depo.TekGetirAsync(id);
        if (hedef is null)
        {
            return IslemSonucu.Basarisiz("Silinecek hedef bulunamadı.");
        }

        depo.SilmeyeIsaretle(hedef);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<HedefKarsilastirmaDTO?> HedefeUzaklikHesaplaAsync(int ogrenciId)
    {
        var hedefDepo = _unitOfWork.RepositoryGetir<Hedef>();
        var aktifHedef = await hedefDepo.Sorgu
            .Include(h => h.DersNetleri).ThenInclude(d => d.Ders)
            .FirstOrDefaultAsync(h => h.OgrenciId == ogrenciId && h.AktifMi);

        if (aktifHedef is null)
        {
            return null;
        }

        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var ogrenciSonuclari = await sonucDepo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi)
            .Include(s => s.Deneme)
            .Include(s => s.Detaylar).ThenInclude(d => d.SinavTuruTest)
            .OrderByDescending(s => s.Deneme.Tarih)
            .ToListAsync();

        var karsilastirmalar = new List<DersHedefKarsilastirmaDTO>();
        foreach (var dersNeti in aktifHedef.DersNetleri)
        {
            // dersin en son hangi denemede sınavı verildiyse o netini alıyoruz, sınav türü fark etmiyor
            DenemeSonucDetay? enSonDetay = null;
            foreach (var sonuc in ogrenciSonuclari)
            {
                enSonDetay = sonuc.Detaylar.FirstOrDefault(d => d.SinavTuruTest.DersId == dersNeti.DersId);
                if (enSonDetay is not null)
                {
                    break;
                }
            }

            var guncelNet = enSonDetay is null
                ? (decimal?)null
                : NetHesaplayici.Hesapla(enSonDetay.Dogru, enSonDetay.Yanlis);

            karsilastirmalar.Add(new DersHedefKarsilastirmaDTO
            {
                DersAdi = dersNeti.Ders?.Ad ?? string.Empty,
                HedefNet = dersNeti.HedefNet,
                GuncelNet = guncelNet,
                Uzaklik = guncelNet.HasValue ? guncelNet.Value - dersNeti.HedefNet : null
            });
        }

        return new HedefKarsilastirmaDTO
        {
            DersKarsilastirmalari = karsilastirmalar,
            ToplamHedefNet = karsilastirmalar.Sum(k => k.HedefNet),
            ToplamGuncelNet = karsilastirmalar.Any(k => k.GuncelNet.HasValue)
                ? karsilastirmalar.Where(k => k.GuncelNet.HasValue).Sum(k => k.GuncelNet!.Value)
                : null
        };
    }

    private void DersNetleriniEslestir(List<HedefDersNetiKaydetDTO> gelenDersNetleri, Hedef mevcutHedef)
    {
        var dersNetiDepo = _unitOfWork.RepositoryGetir<HedefDersNeti>();

        var gelenIdler = gelenDersNetleri.Where(d => d.Id != 0).Select(d => d.Id).ToHashSet();
        foreach (var kaldirilan in mevcutHedef.DersNetleri.Where(d => !gelenIdler.Contains(d.Id)).ToList())
        {
            dersNetiDepo.SilmeyeIsaretle(kaldirilan);
        }

        foreach (var dersNetiDto in gelenDersNetleri)
        {
            if (dersNetiDto.Id == 0)
            {
                mevcutHedef.DersNetleri.Add(dersNetiDto.Entity());
                continue;
            }

            var mevcutDersNeti = mevcutHedef.DersNetleri.FirstOrDefault(d => d.Id == dersNetiDto.Id);
            if (mevcutDersNeti is not null)
            {
                dersNetiDto.Uygula(mevcutDersNeti);
            }
        }
    }
}
