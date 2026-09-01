using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Konu;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class KonuServisi : IKonuServisi
{
    // zayıf konu önerisi listesinde en fazla bu kadar konu gösteriliyor
    private const int OnerilecekKonuSayisi = 5;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<KonuKaydetDTO> _konuDogrulayici;
    private readonly IValidator<OgrenciKonuTakipKaydetDTO> _takipDogrulayici;
    private readonly IDenemeAnaliziServisi _denemeAnaliziServisi;

    public KonuServisi(
        IUnitOfWork unitOfWork,
        IValidator<KonuKaydetDTO> konuDogrulayici,
        IValidator<OgrenciKonuTakipKaydetDTO> takipDogrulayici,
        IDenemeAnaliziServisi denemeAnaliziServisi)
    {
        _unitOfWork = unitOfWork;
        _konuDogrulayici = konuDogrulayici;
        _takipDogrulayici = takipDogrulayici;
        _denemeAnaliziServisi = denemeAnaliziServisi;
    }

    public async Task<List<KonuDTO>> ListeleAsync(int? dersId = null)
    {
        var depo = _unitOfWork.RepositoryGetir<Konu>();

        var sorgu = depo.Sorgu.Include(k => k.Ders).AsQueryable();
        if (dersId.HasValue)
        {
            sorgu = sorgu.Where(k => k.DersId == dersId.Value);
        }

        var konular = await sorgu.OrderBy(k => k.Sira).ToListAsync();

        return konular.Select(k => k.Dto()).ToList();
    }

    public async Task<KonuDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Konu>();

        var konu = await depo.Sorgu
            .Include(k => k.Ders)
            .FirstOrDefaultAsync(k => k.Id == id);

        return konu?.Dto();
    }

    public async Task<KonuKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Konu>();
        var konu = await depo.TekGetirAsync(id);

        return konu?.KaydetDto();
    }

    public async Task<IslemSonucu<KonuDTO>> EkleAsync(KonuKaydetDTO dto)
    {
        var dogrulamaSonucu = await _konuDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<KonuDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Konu>();
        var konu = dto.Entity();

        await depo.EkleAsync(konu);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<KonuDTO>.Basar(konu.Dto());
    }

    public async Task<IslemSonucu<KonuDTO>> GuncelleAsync(KonuKaydetDTO dto)
    {
        var dogrulamaSonucu = await _konuDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<KonuDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Konu>();
        var mevcutKonu = await depo.TekGetirAsync(dto.Id);
        if (mevcutKonu is null)
        {
            return IslemSonucu<KonuDTO>.Basarisiz("Güncellenecek konu bulunamadı.");
        }

        dto.Uygula(mevcutKonu);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<KonuDTO>.Basar(mevcutKonu.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Konu>();
        var konu = await depo.TekGetirAsync(id);
        if (konu is null)
        {
            return IslemSonucu.Basarisiz("Silinecek konu bulunamadı.");
        }

        depo.SilmeyeIsaretle(konu);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<OgrenciKonuTakipDTO>> TakipListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciKonuTakip>();

        var takipler = await depo.Sorgu
            .Where(t => t.OgrenciId == ogrenciId)
            .Include(t => t.Konu).ThenInclude(k => k.Ders)
            .ToListAsync();

        return takipler.Select(t => t.Dto()).ToList();
    }

    public async Task<List<OgrenciKonuTakipDTO>> TumKonularIcinTakipDurumlariniGetirAsync(int ogrenciId)
    {
        var konuDepo = _unitOfWork.RepositoryGetir<Konu>();
        var tumKonular = await konuDepo.Sorgu
            .Where(k => k.AktifMi)
            .Include(k => k.Ders)
            .OrderBy(k => k.Ders.Sira).ThenBy(k => k.Sira)
            .ToListAsync();

        var takipDepo = _unitOfWork.RepositoryGetir<OgrenciKonuTakip>();
        var takipler = await takipDepo.Sorgu
            .Where(t => t.OgrenciId == ogrenciId)
            .ToListAsync();

        return tumKonular.Select(konu =>
        {
            var takip = takipler.FirstOrDefault(t => t.KonuId == konu.Id);

            return new OgrenciKonuTakipDTO
            {
                Id = takip?.Id ?? 0,
                OgrenciId = ogrenciId,
                KonuId = konu.Id,
                KonuAdi = konu.Ad,
                DersId = konu.DersId,
                DersAdi = konu.Ders.Ad,
                Durum = takip?.Durum ?? KonuTakipDurumu.Baslanmadi,
                GuncellemeTarihi = takip?.GuncellemeTarihi ?? konu.OlusturmaTarihi,
                Notlar = takip?.Notlar
            };
        }).ToList();
    }

    public async Task<IslemSonucu<OgrenciKonuTakipDTO>> DurumIsaretleAsync(OgrenciKonuTakipKaydetDTO dto)
    {
        var dogrulamaSonucu = await _takipDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciKonuTakipDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<OgrenciKonuTakip>();

        var mevcutTakip = await depo.BirIleGetirAsync(t => t.OgrenciId == dto.OgrenciId && t.KonuId == dto.KonuId);

        if (mevcutTakip is null)
        {
            var yeniTakip = dto.Entity();
            await depo.EkleAsync(yeniTakip);
            await _unitOfWork.KaydetAsync();
            return IslemSonucu<OgrenciKonuTakipDTO>.Basar(yeniTakip.Dto());
        }

        dto.Uygula(mevcutTakip);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciKonuTakipDTO>.Basar(mevcutTakip.Dto());
    }

    public async Task<List<DersIlerlemeDTO>> DersBazliIlerlemeHesaplaAsync(int ogrenciId)
    {
        var konuDepo = _unitOfWork.RepositoryGetir<Konu>();
        var tumKonular = await konuDepo.Sorgu
            .Where(k => k.AktifMi)
            .Include(k => k.Ders)
            .ToListAsync();

        var takipDepo = _unitOfWork.RepositoryGetir<OgrenciKonuTakip>();
        var takipler = await takipDepo.Sorgu
            .Where(t => t.OgrenciId == ogrenciId)
            .ToListAsync();

        var sonuc = new List<DersIlerlemeDTO>();
        foreach (var grup in tumKonular.GroupBy(k => k.DersId))
        {
            var konuIdleri = grup.Select(k => k.Id).ToHashSet();
            var toplam = grup.Count();
            var tamamlanan = takipler.Count(t => konuIdleri.Contains(t.KonuId) && t.Durum == KonuTakipDurumu.Tamamlandi);

            sonuc.Add(new DersIlerlemeDTO
            {
                DersId = grup.Key,
                DersAdi = grup.First().Ders.Ad,
                ToplamKonuSayisi = toplam,
                TamamlananKonuSayisi = tamamlanan,
                IlerlemeYuzdesi = YuzdeHesaplayici.Hesapla(tamamlanan, toplam)
            });
        }

        return sonuc.OrderBy(s => s.DersAdi).ToList();
    }

    public async Task<List<KonuDTO>> ZayifKonuOnerileriHesaplaAsync(int ogrenciId)
    {
        var sinavTurleri = await _denemeAnaliziServisi.OgrenciKatildigiSinavTurleriniListeleAsync(ogrenciId);

        var zayifDersAdlari = new HashSet<string>();
        foreach (var sinavTuru in sinavTurleri)
        {
            var analiz = await _denemeAnaliziServisi.AnaliziHesaplaAsync(ogrenciId, sinavTuru.Id);
            foreach (var dersAdi in analiz.ZayifAlanlar)
            {
                zayifDersAdlari.Add(dersAdi);
            }
        }

        if (zayifDersAdlari.Count == 0)
        {
            return [];
        }

        var konuDepo = _unitOfWork.RepositoryGetir<Konu>();
        var adaylar = await konuDepo.Sorgu
            .Where(k => k.AktifMi)
            .Include(k => k.Ders)
            .ToListAsync();

        var takipDepo = _unitOfWork.RepositoryGetir<OgrenciKonuTakip>();
        var takipler = await takipDepo.Sorgu
            .Where(t => t.OgrenciId == ogrenciId)
            .ToListAsync();

        // zayıf çıkan derslerden, henüz tamamlanmamış veya tekrar gereken konuları öneriyoruz
        var onerilecekler = adaylar
            .Where(k => zayifDersAdlari.Contains(k.Ders.Ad))
            .Where(k =>
            {
                var takip = takipler.FirstOrDefault(t => t.KonuId == k.Id);
                return takip is null || takip.Durum is KonuTakipDurumu.TekrarGerekli or KonuTakipDurumu.Baslanmadi;
            })
            .OrderBy(k => k.Ders.Ad).ThenBy(k => k.Sira)
            .Take(OnerilecekKonuSayisi)
            .ToList();

        return onerilecekler.Select(k => k.Dto()).ToList();
    }
}
