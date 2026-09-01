using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class GorevServisi : IGorevServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GorevKaydetDTO> _dogrulayici;

    public GorevServisi(IUnitOfWork unitOfWork, IValidator<GorevKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<GorevDTO>> ListeleAsync(GorevFiltreDTO? filtre = null)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();

        var sorgu = depo.Sorgu.Include(g => g.Ogrenci).AsQueryable();

        if (filtre is not null)
        {
            if (filtre.OgrenciId.HasValue)
            {
                sorgu = sorgu.Where(g => g.OgrenciId == filtre.OgrenciId.Value);
            }

            if (filtre.TamamlandiMi.HasValue)
            {
                sorgu = sorgu.Where(g => g.TamamlandiMi == filtre.TamamlandiMi.Value);
            }
        }

        var gorevler = await sorgu.OrderBy(g => g.TamamlandiMi).ThenBy(g => g.Tarih).ToListAsync();

        return gorevler.Select(g => g.Dto()).ToList();
    }

    public async Task<GorevDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();

        var gorev = await depo.Sorgu
            .Include(g => g.Ogrenci)
            .FirstOrDefaultAsync(g => g.Id == id);

        return gorev?.Dto();
    }

    public async Task<GorevKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();
        var gorev = await depo.TekGetirAsync(id);

        return gorev?.KaydetDto();
    }

    public async Task<IslemSonucu<GorevDTO>> EkleAsync(GorevKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<GorevDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Gorev>();
        var gorev = dto.Entity();

        await depo.EkleAsync(gorev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<GorevDTO>.Basar(gorev.Dto());
    }

    public async Task<IslemSonucu<GorevDTO>> GuncelleAsync(GorevKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<GorevDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Gorev>();
        var mevcutGorev = await depo.TekGetirAsync(dto.Id);
        if (mevcutGorev is null)
        {
            return IslemSonucu<GorevDTO>.Basarisiz("Güncellenecek görev bulunamadı.");
        }

        dto.Uygula(mevcutGorev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<GorevDTO>.Basar(mevcutGorev.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();
        var gorev = await depo.TekGetirAsync(id);
        if (gorev is null)
        {
            return IslemSonucu.Basarisiz("Silinecek görev bulunamadı.");
        }

        depo.SilmeyeIsaretle(gorev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<IslemSonucu> TamamlandiIsaretleAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();
        var gorev = await depo.TekGetirAsync(id);
        if (gorev is null)
        {
            return IslemSonucu.Basarisiz("Görev bulunamadı.");
        }

        gorev.TamamlandiMi = true;
        depo.Guncelle(gorev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<GorevDTO>> BugunkuGorevleriListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<Gorev>();

        var gorevler = await depo.Sorgu
            .Include(g => g.Ogrenci)
            .Where(g => g.Tarih.Date == DateTime.Today && g.OgrenciId != null && !g.TamamlandiMi)
            .OrderBy(g => g.Ogrenci!.Ad).ThenBy(g => g.Ogrenci!.Soyad)
            .ToListAsync();

        return gorevler.Select(g => g.Dto()).ToList();
    }
}
