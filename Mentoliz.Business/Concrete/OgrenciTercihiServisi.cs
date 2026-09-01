using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Tercih;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class OgrenciTercihiServisi : IOgrenciTercihiServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<OgrenciTercihiKaydetDTO> _dogrulayici;

    public OgrenciTercihiServisi(IUnitOfWork unitOfWork, IValidator<OgrenciTercihiKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<OgrenciTercihiDTO>> ListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciTercihi>();

        var tercihler = await depo.Sorgu
            .Where(t => t.OgrenciId == ogrenciId)
            .OrderBy(t => t.Sira)
            .ToListAsync();

        return tercihler.Select(t => t.Dto()).ToList();
    }

    public async Task<OgrenciTercihiKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciTercihi>();
        var tercih = await depo.TekGetirAsync(id);

        return tercih?.KaydetDto();
    }

    public async Task<IslemSonucu<OgrenciTercihiDTO>> EkleAsync(OgrenciTercihiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciTercihiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<OgrenciTercihi>();
        var tercih = dto.Entity();

        await depo.EkleAsync(tercih);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciTercihiDTO>.Basar(tercih.Dto());
    }

    public async Task<IslemSonucu<OgrenciTercihiDTO>> GuncelleAsync(OgrenciTercihiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciTercihiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<OgrenciTercihi>();
        var mevcutTercih = await depo.TekGetirAsync(dto.Id);
        if (mevcutTercih is null)
        {
            return IslemSonucu<OgrenciTercihiDTO>.Basarisiz("Güncellenecek tercih bulunamadı.");
        }

        dto.Uygula(mevcutTercih);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciTercihiDTO>.Basar(mevcutTercih.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciTercihi>();
        var tercih = await depo.TekGetirAsync(id);
        if (tercih is null)
        {
            return IslemSonucu.Basarisiz("Silinecek tercih bulunamadı.");
        }

        depo.SilmeyeIsaretle(tercih);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
