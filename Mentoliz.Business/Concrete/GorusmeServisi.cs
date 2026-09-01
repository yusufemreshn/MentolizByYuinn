using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class GorusmeServisi : IGorusmeServisi
{
    // bu süreden uzun süredir görüşülmeyen aktif öğrenciler ana sayfada uyarı olarak listeleniyor
    private const int UzunSureEsikGunSayisi = 30;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GorusmeKaydetDTO> _dogrulayici;
    private readonly IGorevServisi _gorevServisi;

    public GorusmeServisi(IUnitOfWork unitOfWork, IValidator<GorusmeKaydetDTO> dogrulayici, IGorevServisi gorevServisi)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
        _gorevServisi = gorevServisi;
    }

    public async Task<List<GorusmeDTO>> ListeleAsync(GorusmeFiltreDTO? filtre = null)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorusme>();

        var sorgu = depo.Sorgu.Include(g => g.Ogrenci).AsQueryable();

        if (filtre is not null)
        {
            if (filtre.OgrenciId.HasValue)
            {
                sorgu = sorgu.Where(g => g.OgrenciId == filtre.OgrenciId.Value);
            }

            if (filtre.Tur.HasValue)
            {
                sorgu = sorgu.Where(g => g.Tur == filtre.Tur.Value);
            }
        }

        var gorusmeler = await sorgu.OrderByDescending(g => g.Tarih).ToListAsync();

        return gorusmeler.Select(g => g.Dto()).ToList();
    }

    public async Task<GorusmeDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorusme>();

        var gorusme = await depo.Sorgu
            .Include(g => g.Ogrenci)
            .FirstOrDefaultAsync(g => g.Id == id);

        return gorusme?.Dto();
    }

    public async Task<GorusmeKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorusme>();
        var gorusme = await depo.TekGetirAsync(id);

        return gorusme?.KaydetDto();
    }

    public async Task<IslemSonucu<GorusmeDTO>> EkleAsync(GorusmeKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<GorusmeDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Gorusme>();
        var gorusme = dto.Entity();

        await depo.EkleAsync(gorusme);
        await _unitOfWork.KaydetAsync();

        if (dto.SonrakiGorusmeTarihi.HasValue)
        {
            await OtomatikGorevOlusturAsync(dto);
        }

        return IslemSonucu<GorusmeDTO>.Basar(gorusme.Dto());
    }

    public async Task<IslemSonucu<GorusmeDTO>> GuncelleAsync(GorusmeKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<GorusmeDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Gorusme>();
        var mevcutGorusme = await depo.TekGetirAsync(dto.Id);
        if (mevcutGorusme is null)
        {
            return IslemSonucu<GorusmeDTO>.Basarisiz("Güncellenecek görüşme bulunamadı.");
        }

        dto.Uygula(mevcutGorusme);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<GorusmeDTO>.Basar(mevcutGorusme.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Gorusme>();
        var gorusme = await depo.TekGetirAsync(id);
        if (gorusme is null)
        {
            return IslemSonucu.Basarisiz("Silinecek görüşme bulunamadı.");
        }

        depo.SilmeyeIsaretle(gorusme);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<UzunSureGorusulmeyenOgrenciDTO>> UzunSureGorusulmeyenleriListeleAsync()
    {
        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var aktifOgrenciler = await ogrenciDepo.Sorgu
            .Where(o => o.AktifMi)
            .Include(o => o.Gorusmeler)
            .ToListAsync();

        var esikTarih = DateTime.Today.AddDays(-UzunSureEsikGunSayisi);

        var sonuc = new List<UzunSureGorusulmeyenOgrenciDTO>();
        foreach (var ogrenci in aktifOgrenciler)
        {
            var sonGorusme = ogrenci.Gorusmeler.OrderByDescending(g => g.Tarih).FirstOrDefault();
            if (sonGorusme is not null && sonGorusme.Tarih.Date >= esikTarih)
            {
                continue;
            }

            sonuc.Add(new UzunSureGorusulmeyenOgrenciDTO
            {
                OgrenciId = ogrenci.Id,
                OgrenciAdSoyad = $"{ogrenci.Ad} {ogrenci.Soyad}",
                SonGorusmeTarihi = sonGorusme?.Tarih,
                GecenGunSayisi = sonGorusme is null ? null : (int)(DateTime.Today - sonGorusme.Tarih.Date).TotalDays
            });
        }

        return sonuc.OrderByDescending(s => s.GecenGunSayisi ?? int.MaxValue).ToList();
    }

    // görüşmede sonraki tarih girilince öğretmenin unutmaması için kendiliğinden bir görev açıyoruz
    private async Task OtomatikGorevOlusturAsync(GorusmeKaydetDTO dto)
    {
        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var ogrenci = await ogrenciDepo.TekGetirAsync(dto.OgrenciId);

        var turMetni = dto.Tur switch
        {
            GorusmeTuru.VeliGorusmesi => "veli görüşmesi",
            GorusmeTuru.OgretmenGorusmesi => "öğretmen görüşmesi",
            _ => "öğrenci görüşmesi"
        };

        var ogrenciAdi = ogrenci is null ? "" : $"{ogrenci.Ad} {ogrenci.Soyad} ";

        await _gorevServisi.EkleAsync(new GorevKaydetDTO
        {
            Baslik = $"{ogrenciAdi}ile {turMetni}",
            Tarih = dto.SonrakiGorusmeTarihi!.Value,
            OgrenciId = dto.OgrenciId,
            Oncelik = GorevOnceligi.Normal,
            TamamlandiMi = false
        });
    }
}
