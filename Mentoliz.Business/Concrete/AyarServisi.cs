using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class AyarServisi : IAyarServisi
{
    private readonly IUnitOfWork _unitOfWork;

    public AyarServisi(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> DegerGetirAsync(string anahtar)
    {
        var depo = _unitOfWork.RepositoryGetir<Ayar>();
        var ayar = await depo.BirIleGetirAsync(a => a.Anahtar == anahtar);
        return ayar?.Deger;
    }

    public async Task<IslemSonucu> DegerAtaAsync(string anahtar, string deger)
    {
        if (string.IsNullOrWhiteSpace(anahtar))
        {
            return IslemSonucu.Basarisiz("Ayar anahtarı boş olamaz.");
        }

        var depo = _unitOfWork.RepositoryGetir<Ayar>();
        var mevcutAyar = await depo.BirIleGetirAsync(a => a.Anahtar == anahtar);

        if (mevcutAyar is null)
        {
            await depo.EkleAsync(new Ayar { Anahtar = anahtar, Deger = deger });
        }
        else
        {
            mevcutAyar.Deger = deger;
            depo.Guncelle(mevcutAyar);
        }

        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
