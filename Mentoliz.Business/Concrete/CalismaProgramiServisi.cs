using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.CalismaProgrami;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class CalismaProgramiServisi : ICalismaProgramiServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CalismaProgramiKaydetDTO> _dogrulayici;

    public CalismaProgramiServisi(IUnitOfWork unitOfWork, IValidator<CalismaProgramiKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<CalismaProgramiDTO>> ListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();

        var programlar = await depo.Sorgu
            .Where(c => c.OgrenciId == ogrenciId)
            .Include(c => c.Satirlar).ThenInclude(s => s.Ders)
            .OrderByDescending(c => c.HaftaBaslangicTarihi)
            .ToListAsync();

        return programlar.Select(c => c.Dto()).ToList();
    }

    public async Task<CalismaProgramiDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();

        var program = await depo.Sorgu
            .Include(c => c.Satirlar).ThenInclude(s => s.Ders)
            .FirstOrDefaultAsync(c => c.Id == id);

        return program?.Dto();
    }

    public async Task<CalismaProgramiKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();

        var program = await depo.Sorgu
            .Include(c => c.Satirlar)
            .FirstOrDefaultAsync(c => c.Id == id);

        return program?.KaydetDto();
    }

    public async Task<IslemSonucu<CalismaProgramiDTO>> EkleAsync(CalismaProgramiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<CalismaProgramiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();
        var program = dto.Entity();
        foreach (var satirDto in dto.Satirlar)
        {
            program.Satirlar.Add(satirDto.Entity());
        }

        await depo.EkleAsync(program);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<CalismaProgramiDTO>.Basar(program.Dto());
    }

    public async Task<IslemSonucu<CalismaProgramiDTO>> GuncelleAsync(CalismaProgramiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<CalismaProgramiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();

        var mevcutProgram = await depo.Sorgu
            .Include(c => c.Satirlar)
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (mevcutProgram is null)
        {
            return IslemSonucu<CalismaProgramiDTO>.Basarisiz("Güncellenecek çalışma programı bulunamadı.");
        }

        dto.Uygula(mevcutProgram);
        SatirlariEslestir(dto.Satirlar, mevcutProgram);

        await _unitOfWork.KaydetAsync();

        return IslemSonucu<CalismaProgramiDTO>.Basar(mevcutProgram.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();
        var program = await depo.TekGetirAsync(id);
        if (program is null)
        {
            return IslemSonucu.Basarisiz("Silinecek çalışma programı bulunamadı.");
        }

        depo.SilmeyeIsaretle(program);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<IslemSonucu<CalismaProgramiSatiriDTO>> SatirEkleAsync(int calismaProgramiId, CalismaProgramiSatiriKaydetDTO satir)
    {
        var programDepo = _unitOfWork.RepositoryGetir<CalismaProgrami>();
        var program = await programDepo.TekGetirAsync(calismaProgramiId);
        if (program is null)
        {
            return IslemSonucu<CalismaProgramiSatiriDTO>.Basarisiz("Satırın ekleneceği çalışma programı bulunamadı.");
        }

        var satirDepo = _unitOfWork.RepositoryGetir<CalismaProgramiSatiri>();
        var yeniSatir = satir.Entity();
        yeniSatir.CalismaProgramiId = calismaProgramiId;

        await satirDepo.EkleAsync(yeniSatir);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<CalismaProgramiSatiriDTO>.Basar(yeniSatir.Dto());
    }

    public async Task<IslemSonucu> SatirSilAsync(int satirId)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgramiSatiri>();
        var satir = await depo.TekGetirAsync(satirId);
        if (satir is null)
        {
            return IslemSonucu.Basarisiz("Silinecek satır bulunamadı.");
        }

        depo.SilmeyeIsaretle(satir);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<IslemSonucu> SatirTamamlandiIsaretleAsync(int satirId, bool tamamlandiMi)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgramiSatiri>();
        var satir = await depo.TekGetirAsync(satirId);
        if (satir is null)
        {
            return IslemSonucu.Basarisiz("Güncellenecek satır bulunamadı.");
        }

        satir.TamamlandiMi = tamamlandiMi;
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<IslemSonucu<CalismaProgramiDTO>> OncekiHaftadanKopyalaAsync(int ogrenciId, DateTime yeniHaftaBaslangicTarihi)
    {
        var depo = _unitOfWork.RepositoryGetir<CalismaProgrami>();

        var oncekiHaftaTarihi = yeniHaftaBaslangicTarihi.AddDays(-7);
        var oncekiProgram = await depo.Sorgu
            .Include(c => c.Satirlar)
            .FirstOrDefaultAsync(c => c.OgrenciId == ogrenciId && c.HaftaBaslangicTarihi.Date == oncekiHaftaTarihi.Date);

        if (oncekiProgram is null)
        {
            return IslemSonucu<CalismaProgramiDTO>.Basarisiz("Önceki hafta için kopyalanacak bir çalışma programı bulunamadı.");
        }

        var yeniVarMi = await depo.VarMiAsync(c => c.OgrenciId == ogrenciId && c.HaftaBaslangicTarihi.Date == yeniHaftaBaslangicTarihi.Date);
        if (yeniVarMi)
        {
            return IslemSonucu<CalismaProgramiDTO>.Basarisiz("Bu hafta için zaten bir çalışma programı var.");
        }

        var yeniProgram = new CalismaProgrami
        {
            OgrenciId = ogrenciId,
            HaftaBaslangicTarihi = yeniHaftaBaslangicTarihi,
            Aciklama = oncekiProgram.Aciklama
        };

        // tamamlandı işaretleri yeni haftada sıfırdan başlıyor
        foreach (var oncekiSatir in oncekiProgram.Satirlar)
        {
            yeniProgram.Satirlar.Add(new CalismaProgramiSatiri
            {
                Gun = oncekiSatir.Gun,
                BaslangicSaati = oncekiSatir.BaslangicSaati,
                BitisSaati = oncekiSatir.BitisSaati,
                DersId = oncekiSatir.DersId,
                Aciklama = oncekiSatir.Aciklama,
                TamamlandiMi = false
            });
        }

        await depo.EkleAsync(yeniProgram);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<CalismaProgramiDTO>.Basar(yeniProgram.Dto());
    }

    private void SatirlariEslestir(List<CalismaProgramiSatiriKaydetDTO> gelenSatirlar, CalismaProgrami mevcutProgram)
    {
        var satirDepo = _unitOfWork.RepositoryGetir<CalismaProgramiSatiri>();

        var gelenIdler = gelenSatirlar.Where(s => s.Id != 0).Select(s => s.Id).ToHashSet();
        foreach (var kaldirilan in mevcutProgram.Satirlar.Where(s => !gelenIdler.Contains(s.Id)).ToList())
        {
            satirDepo.SilmeyeIsaretle(kaldirilan);
        }

        foreach (var satirDto in gelenSatirlar)
        {
            if (satirDto.Id == 0)
            {
                mevcutProgram.Satirlar.Add(satirDto.Entity());
                continue;
            }

            var mevcutSatir = mevcutProgram.Satirlar.FirstOrDefault(s => s.Id == satirDto.Id);
            if (mevcutSatir is not null)
            {
                satirDto.Uygula(mevcutSatir);
            }
        }
    }
}
