using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Ders;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class DersServisi : IDersServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DersKaydetDTO> _dogrulayici;

    public DersServisi(IUnitOfWork unitOfWork, IValidator<DersKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<DersDTO>> ListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Ders>();
        var dersler = await depo.Sorgu.OrderBy(d => d.Sira).ToListAsync();
        return dersler.Select(d => d.Dto()).ToList();
    }

    public async Task<DersDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Ders>();
        var ders = await depo.TekGetirAsync(id);
        return ders?.Dto();
    }

    public async Task<IslemSonucu<DersDTO>> EkleAsync(DersKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DersDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Ders>();
        var ders = dto.Entity();

        await depo.EkleAsync(ders);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DersDTO>.Basar(ders.Dto());
    }

    public async Task<IslemSonucu<DersDTO>> GuncelleAsync(DersKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DersDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Ders>();
        var mevcutDers = await depo.TekGetirAsync(dto.Id);
        if (mevcutDers is null)
        {
            return IslemSonucu<DersDTO>.Basarisiz("Güncellenecek ders bulunamadı.");
        }

        dto.Uygula(mevcutDers);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DersDTO>.Basar(mevcutDers.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Ders>();
        var ders = await depo.TekGetirAsync(id);
        if (ders is null)
        {
            return IslemSonucu.Basarisiz("Silinecek ders bulunamadı.");
        }

        depo.SilmeyeIsaretle(ders);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
