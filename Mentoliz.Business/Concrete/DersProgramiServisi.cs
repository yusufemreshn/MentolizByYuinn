using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class DersProgramiServisi : IDersProgramiServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DersProgramiSatiriKaydetDTO> _dogrulayici;

    public DersProgramiServisi(IUnitOfWork unitOfWork, IValidator<DersProgramiSatiriKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<DersProgramiSatiriDTO>> ListeleAsync(int sinifId)
    {
        var depo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();

        // sqlite timespan alanını sırlamayı desteklemiyor, saat sıralamasını bellekte yapıyoruz
        var satirlar = await depo.Sorgu
            .Where(s => s.SinifId == sinifId)
            .Include(s => s.Ders)
            .ToListAsync();

        return satirlar
            .OrderBy(s => s.Gun).ThenBy(s => s.BaslangicSaati)
            .Select(s => s.Dto())
            .ToList();
    }

    public async Task<DersProgramiSatiriKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();
        var satir = await depo.TekGetirAsync(id);

        return satir?.KaydetDto();
    }

    public async Task<IslemSonucu<DersProgramiSatiriDTO>> EkleAsync(DersProgramiSatiriKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DersProgramiSatiriDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();
        var satir = dto.Entity();

        await depo.EkleAsync(satir);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DersProgramiSatiriDTO>.Basar(satir.Dto());
    }

    public async Task<IslemSonucu<DersProgramiSatiriDTO>> GuncelleAsync(DersProgramiSatiriKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<DersProgramiSatiriDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();
        var mevcutSatir = await depo.TekGetirAsync(dto.Id);
        if (mevcutSatir is null)
        {
            return IslemSonucu<DersProgramiSatiriDTO>.Basarisiz("Güncellenecek ders programı satırı bulunamadı.");
        }

        dto.Uygula(mevcutSatir);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<DersProgramiSatiriDTO>.Basar(mevcutSatir.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();
        var satir = await depo.TekGetirAsync(id);
        if (satir is null)
        {
            return IslemSonucu.Basarisiz("Silinecek ders programı satırı bulunamadı.");
        }

        depo.SilmeyeIsaretle(satir);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
