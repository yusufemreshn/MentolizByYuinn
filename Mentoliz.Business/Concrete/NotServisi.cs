using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Not;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class NotServisi : INotServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<NotKaydetDTO> _dogrulayici;

    public NotServisi(IUnitOfWork unitOfWork, IValidator<NotKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<NotDTO>> ListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Not>();

        // en son güncellenen not en üstte görünsün diye
        var notlar = await depo.Sorgu.OrderByDescending(n => n.GuncellemeTarihi).ToListAsync();

        return notlar.Select(n => n.Dto()).ToList();
    }

    public async Task<NotKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Not>();
        var not = await depo.TekGetirAsync(id);

        return not?.KaydetDto();
    }

    public async Task<IslemSonucu<NotDTO>> EkleAsync(NotKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<NotDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Not>();
        var not = dto.Entity();

        await depo.EkleAsync(not);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<NotDTO>.Basar(not.Dto());
    }

    public async Task<IslemSonucu<NotDTO>> GuncelleAsync(NotKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<NotDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Not>();
        var mevcutNot = await depo.TekGetirAsync(dto.Id);
        if (mevcutNot is null)
        {
            return IslemSonucu<NotDTO>.Basarisiz("Güncellenecek not bulunamadı.");
        }

        dto.Uygula(mevcutNot);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<NotDTO>.Basar(mevcutNot.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Not>();
        var not = await depo.TekGetirAsync(id);
        if (not is null)
        {
            return IslemSonucu.Basarisiz("Silinecek not bulunamadı.");
        }

        depo.SilmeyeIsaretle(not);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
