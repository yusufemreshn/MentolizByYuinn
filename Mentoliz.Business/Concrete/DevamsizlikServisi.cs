using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Dto.Devamsizlik;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class DevamsizlikServisi : IDevamsizlikServisi
{
    // öğrenci detay sayfasındaki ve risk hesaplamasındaki özet bu kadar geriye bakıyor
    private const int OzetGunSayisi = 30;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IOgrenciServisi _ogrenciServisi;

    public DevamsizlikServisi(IUnitOfWork unitOfWork, IOgrenciServisi ogrenciServisi)
    {
        _unitOfWork = unitOfWork;
        _ogrenciServisi = ogrenciServisi;
    }

    public async Task<SinifDevamsizlikGunuDTO> SinifGunlukDurumGetirAsync(int sinifId, DateTime tarih)
    {
        var gun = tarih.Date;

        var ogrenciler = await _ogrenciServisi.ListeleAsync(new OgrenciFiltreDTO { SinifId = sinifId, AktifMi = true });

        var depo = _unitOfWork.RepositoryGetir<OgrenciDevamsizlik>();
        var oGuninKayitlari = await depo.Sorgu
            .Where(d => d.Tarih == gun && ogrenciler.Select(o => o.Id).Contains(d.OgrenciId))
            .ToListAsync();

        var model = new SinifDevamsizlikGunuDTO
        {
            SinifId = sinifId,
            Tarih = gun,
            Ogrenciler = ogrenciler
                .OrderBy(o => o.Ad).ThenBy(o => o.Soyad)
                .Select(o =>
                {
                    var kayit = oGuninKayitlari.FirstOrDefault(d => d.OgrenciId == o.Id);
                    return new OgrenciDevamsizlikDurumuDTO
                    {
                        OgrenciId = o.Id,
                        OgrenciAdSoyad = o.AdSoyad,
                        Durum = kayit?.Durum ?? DevamsizlikDurumu.Geldi,
                        Not = kayit?.Not
                    };
                })
                .ToList()
        };

        return model;
    }

    public async Task<IslemSonucu> DurumKaydetAsync(DevamsizlikKaydetDTO dto)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciDevamsizlik>();
        var gun = dto.Tarih.Date;

        var mevcutKayit = await depo.BirIleGetirAsync(d => d.OgrenciId == dto.OgrenciId && d.Tarih == gun);

        // geldi seçilmesi, o gün için istisna kaydı kalmaması gerektiği anlamına geliyor
        if (dto.Durum == DevamsizlikDurumu.Geldi)
        {
            if (mevcutKayit is not null)
            {
                depo.SilmeyeIsaretle(mevcutKayit);
                await _unitOfWork.KaydetAsync();
            }

            return IslemSonucu.Basar();
        }

        if (mevcutKayit is not null)
        {
            dto.Uygula(mevcutKayit);
        }
        else
        {
            await depo.EkleAsync(dto.Entity());
        }

        await _unitOfWork.KaydetAsync();
        return IslemSonucu.Basar();
    }

    public async Task<OgrenciDevamsizlikOzetiDTO> OgrenciOzetiHesaplaAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciDevamsizlik>();
        var esikTarih = DateTime.Today.AddDays(-OzetGunSayisi);

        var kayitlar = await depo.Sorgu
            .Where(d => d.OgrenciId == ogrenciId && d.Tarih >= esikTarih)
            .ToListAsync();

        return new OgrenciDevamsizlikOzetiDTO
        {
            OgrenciId = ogrenciId,
            GelmemeSayisi = kayitlar.Count(d => d.Durum == DevamsizlikDurumu.Gelmedi),
            MazeretliSayisi = kayitlar.Count(d => d.Durum == DevamsizlikDurumu.Mazeretli)
        };
    }

    public async Task<List<OgrenciDevamsizlikOzetiDTO>> TumOgrencilerOzetiHesaplaAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciDevamsizlik>();
        var esikTarih = DateTime.Today.AddDays(-OzetGunSayisi);

        var kayitlar = await depo.Sorgu
            .Where(d => d.Tarih >= esikTarih)
            .ToListAsync();

        return kayitlar
            .GroupBy(d => d.OgrenciId)
            .Select(grup => new OgrenciDevamsizlikOzetiDTO
            {
                OgrenciId = grup.Key,
                GelmemeSayisi = grup.Count(d => d.Durum == DevamsizlikDurumu.Gelmedi),
                MazeretliSayisi = grup.Count(d => d.Durum == DevamsizlikDurumu.Mazeretli)
            })
            .ToList();
    }
}
