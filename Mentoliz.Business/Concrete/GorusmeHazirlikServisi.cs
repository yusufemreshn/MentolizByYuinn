using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class GorusmeHazirlikServisi : IGorusmeHazirlikServisi
{
    // yan panelde en fazla bu kadar son deneme gösteriliyor
    private const int SonDenemeAdedi = 3;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IOdevServisi _odevServisi;
    private readonly IGorusmeServisi _gorusmeServisi;

    public GorusmeHazirlikServisi(IUnitOfWork unitOfWork, IOdevServisi odevServisi, IGorusmeServisi gorusmeServisi)
    {
        _unitOfWork = unitOfWork;
        _odevServisi = odevServisi;
        _gorusmeServisi = gorusmeServisi;
    }

    public async Task<GorusmeHazirlikOzetiDTO> OzetGetirAsync(int ogrenciId)
    {
        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var sonSonuclar = await sonucDepo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi)
            .Include(s => s.Deneme)
            .Include(s => s.Detaylar)
            .OrderByDescending(s => s.Deneme.Tarih)
            .Take(SonDenemeAdedi)
            .ToListAsync();

        var odevler = await _odevServisi.ListeleAsync(new OdevFiltreDTO { OgrenciId = ogrenciId });
        var acikOdevSayisi = odevler.Count(o => o.Durum is OdevDurumu.Verildi or OdevDurumu.Gecikti);

        var gorusmeler = await _gorusmeServisi.ListeleAsync(new GorusmeFiltreDTO { OgrenciId = ogrenciId });
        var sonGorusme = gorusmeler.OrderByDescending(g => g.Tarih).FirstOrDefault(g => !string.IsNullOrWhiteSpace(g.AlinanKarar));

        return new GorusmeHazirlikOzetiDTO
        {
            SonDenemeNetleri = sonSonuclar
                .OrderBy(s => s.Deneme.Tarih)
                .Select(s => new SonDenemeNetiDTO
                {
                    DenemeAdi = s.Deneme.Ad,
                    Tarih = s.Deneme.Tarih,
                    ToplamNet = s.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis))
                })
                .ToList(),
            AcikOdevSayisi = acikOdevSayisi,
            SonAlinanKarar = sonGorusme?.AlinanKarar,
            SonGorusmeTarihi = sonGorusme?.Tarih
        };
    }
}
