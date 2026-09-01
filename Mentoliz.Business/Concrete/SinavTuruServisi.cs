using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class SinavTuruServisi : ISinavTuruServisi
{
    private readonly IUnitOfWork _unitOfWork;

    public SinavTuruServisi(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SinavTuruDTO>> ListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<SinavTuru>();

        var sinavTurleri = await depo.Sorgu
            .Include(s => s.Testler).ThenInclude(t => t.Ders)
            .OrderBy(s => s.Sira)
            .ToListAsync();

        return sinavTurleri.Select(s => s.Dto()).ToList();
    }

    public async Task<SinavTuruDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<SinavTuru>();

        var sinavTuru = await depo.Sorgu
            .Include(s => s.Testler).ThenInclude(t => t.Ders)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sinavTuru?.Dto();
    }
}
