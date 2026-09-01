using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Ortak;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class TakvimServisi : ITakvimServisi
{
    private readonly IUnitOfWork _unitOfWork;

    public TakvimServisi(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TakvimOgesiDTO>> OgeleriGetirAsync(DateTime baslangic, DateTime bitisDahilDegil)
    {
        var ogeler = new List<TakvimOgesiDTO>();

        var gorusmeDepo = _unitOfWork.RepositoryGetir<Gorusme>();
        var gorusmeler = await gorusmeDepo.Sorgu
            .Where(g => g.Tarih >= baslangic && g.Tarih < bitisDahilDegil)
            .Include(g => g.Ogrenci)
            .ToListAsync();
        ogeler.AddRange(gorusmeler.Select(g => new TakvimOgesiDTO
        {
            Tarih = g.Tarih,
            Tur = "gorusme",
            Baslik = $"{g.Ogrenci.Ad} {g.Ogrenci.Soyad} — {g.Konu}",
            IlgiliId = g.Id
        }));

        // ödev takvimde son teslim tarihinde gösteriliyor, verildiği tarihte değil
        var odevDepo = _unitOfWork.RepositoryGetir<Odev>();
        var odevler = await odevDepo.Sorgu
            .Where(o => o.SonTeslimTarihi >= baslangic && o.SonTeslimTarihi < bitisDahilDegil)
            .Include(o => o.Ogrenci)
            .Include(o => o.Ders)
            .ToListAsync();
        ogeler.AddRange(odevler.Select(o => new TakvimOgesiDTO
        {
            Tarih = o.SonTeslimTarihi,
            Tur = "odev",
            Baslik = $"{o.Ogrenci.Ad} {o.Ogrenci.Soyad} — {o.Ders.Ad} teslimi",
            IlgiliId = o.Id
        }));

        var denemeDepo = _unitOfWork.RepositoryGetir<Deneme>();
        var denemeler = await denemeDepo.Sorgu
            .Where(d => d.Tarih >= baslangic && d.Tarih < bitisDahilDegil)
            .ToListAsync();
        ogeler.AddRange(denemeler.Select(d => new TakvimOgesiDTO
        {
            Tarih = d.Tarih,
            Tur = "deneme",
            Baslik = d.Ad,
            IlgiliId = d.Id
        }));

        var gorevDepo = _unitOfWork.RepositoryGetir<Gorev>();
        var gorevler = await gorevDepo.Sorgu
            .Where(g => g.Tarih >= baslangic && g.Tarih < bitisDahilDegil && !g.TamamlandiMi)
            .ToListAsync();
        ogeler.AddRange(gorevler.Select(g => new TakvimOgesiDTO
        {
            Tarih = g.Tarih,
            Tur = "gorev",
            Baslik = g.Baslik,
            IlgiliId = g.Id
        }));

        return ogeler.OrderBy(o => o.Tarih).ToList();
    }
}
