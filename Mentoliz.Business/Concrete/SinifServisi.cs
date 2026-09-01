using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Sinif;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class SinifServisi : ISinifServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SinifKaydetDTO> _dogrulayici;

    public SinifServisi(IUnitOfWork unitOfWork, IValidator<SinifKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<SinifDTO>> ListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Sinif>();

        var siniflar = await depo.Sorgu
            .Include(s => s.Ogrenciler)
            .OrderBy(s => s.Ad)
            .ToListAsync();

        return siniflar.Select(s => s.Dto()).ToList();
    }

    public async Task<SinifDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Sinif>();

        var sinif = await depo.Sorgu
            .Include(s => s.Ogrenciler)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sinif?.Dto();
    }

    public async Task<SinifKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Sinif>();
        var sinif = await depo.TekGetirAsync(id);

        return sinif?.KaydetDto();
    }

    public async Task<IslemSonucu<SinifDTO>> EkleAsync(SinifKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<SinifDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Sinif>();
        var sinif = dto.Entity();

        await depo.EkleAsync(sinif);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<SinifDTO>.Basar(sinif.Dto());
    }

    public async Task<IslemSonucu<SinifDTO>> GuncelleAsync(SinifKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<SinifDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Sinif>();
        var mevcutSinif = await depo.TekGetirAsync(dto.Id);
        if (mevcutSinif is null)
        {
            return IslemSonucu<SinifDTO>.Basarisiz("Güncellenecek sınıf bulunamadı.");
        }

        dto.Uygula(mevcutSinif);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<SinifDTO>.Basar(mevcutSinif.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Sinif>();
        var sinif = await depo.TekGetirAsync(id);
        if (sinif is null)
        {
            return IslemSonucu.Basarisiz("Silinecek sınıf bulunamadı.");
        }

        // sınıfta öğrenci varken silinirse o öğrenciler sınıfsız kalır, önce başka sınıfa taşınmaları gerekiyor
        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        if (await ogrenciDepo.VarMiAsync(o => o.SinifId == id))
        {
            return IslemSonucu.Basarisiz("Bu sınıfta öğrenci bulunduğu için silinemez, önce öğrencileri başka bir sınıfa taşıyın.");
        }

        depo.SilmeyeIsaretle(sinif);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
