using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class OdevServisi : IOdevServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<OdevKaydetDTO> _odevDogrulayici;
    private readonly IValidator<OdevTopluAtamaKaydetDTO> _topluAtamaDogrulayici;

    public OdevServisi(
        IUnitOfWork unitOfWork,
        IValidator<OdevKaydetDTO> odevDogrulayici,
        IValidator<OdevTopluAtamaKaydetDTO> topluAtamaDogrulayici)
    {
        _unitOfWork = unitOfWork;
        _odevDogrulayici = odevDogrulayici;
        _topluAtamaDogrulayici = topluAtamaDogrulayici;
    }

    public async Task<List<OdevDTO>> ListeleAsync(OdevFiltreDTO? filtre = null)
    {
        var depo = _unitOfWork.RepositoryGetir<Odev>();

        var sorgu = depo.Sorgu
            .Include(o => o.Ogrenci)
            .Include(o => o.Ders)
            .AsQueryable();

        if (filtre is not null)
        {
            if (!string.IsNullOrWhiteSpace(filtre.Arama))
            {
                // like kullanıyoruz çünkü ef core sqlite'ta contains'i büyük küçük harfe duyarlı çeviriyor
                var arama = $"%{filtre.Arama.Trim()}%";
                sorgu = sorgu.Where(o => EF.Functions.Like(o.KonuBasligi, arama));
            }

            if (filtre.OgrenciId.HasValue)
            {
                sorgu = sorgu.Where(o => o.OgrenciId == filtre.OgrenciId.Value);
            }

            if (filtre.SinifId.HasValue)
            {
                sorgu = sorgu.Where(o => o.Ogrenci.SinifId == filtre.SinifId.Value);
            }

            if (filtre.DersId.HasValue)
            {
                sorgu = sorgu.Where(o => o.DersId == filtre.DersId.Value);
            }

            if (filtre.TopluAtamaId.HasValue)
            {
                sorgu = sorgu.Where(o => o.TopluAtamaId == filtre.TopluAtamaId.Value);
            }
        }

        var odevler = await sorgu.OrderByDescending(o => o.SonTeslimTarihi).ToListAsync();
        var sonuc = odevler.Select(o => o.Dto());

        // gecikti durumu okuma anında hesaplandığı için veritabanı sorgusunda filtrelenemiyor, burada eleniyor
        if (filtre?.Durum is not null)
        {
            sonuc = sonuc.Where(o => o.Durum == filtre.Durum.Value);
        }

        return sonuc.ToList();
    }

    public async Task<OdevDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Odev>();

        var odev = await depo.Sorgu
            .Include(o => o.Ogrenci)
            .Include(o => o.Ders)
            .FirstOrDefaultAsync(o => o.Id == id);

        return odev?.Dto();
    }

    public async Task<OdevKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Odev>();
        var odev = await depo.TekGetirAsync(id);

        return odev?.KaydetDto();
    }

    public async Task<IslemSonucu<OdevDTO>> EkleAsync(OdevKaydetDTO dto)
    {
        var dogrulamaSonucu = await _odevDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OdevDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Odev>();
        var odev = dto.Entity();

        await depo.EkleAsync(odev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OdevDTO>.Basar(odev.Dto());
    }

    public async Task<IslemSonucu<OdevDTO>> GuncelleAsync(OdevKaydetDTO dto)
    {
        var dogrulamaSonucu = await _odevDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OdevDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<Odev>();
        var mevcutOdev = await depo.TekGetirAsync(dto.Id);
        if (mevcutOdev is null)
        {
            return IslemSonucu<OdevDTO>.Basarisiz("Güncellenecek ödev bulunamadı.");
        }

        dto.Uygula(mevcutOdev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OdevDTO>.Basar(mevcutOdev.Dto());
    }

    public async Task<IslemSonucu> DurumGuncelleAsync(int id, OdevDurumu yeniDurum)
    {
        var depo = _unitOfWork.RepositoryGetir<Odev>();
        var odev = await depo.TekGetirAsync(id);
        if (odev is null)
        {
            return IslemSonucu.Basarisiz("Durumu güncellenecek ödev bulunamadı.");
        }

        odev.Durum = yeniDurum;

        // tamamlandı veya eksik tamamlandı işaretlenince yapılma tarihi bugün oluyor, geri alınırsa temizleniyor
        odev.YapilmaTarihi = yeniDurum is OdevDurumu.Tamamlandi or OdevDurumu.EksikTamamlandi
            ? DateTime.Today
            : null;

        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Odev>();
        var odev = await depo.TekGetirAsync(id);
        if (odev is null)
        {
            return IslemSonucu.Basarisiz("Silinecek ödev bulunamadı.");
        }

        depo.SilmeyeIsaretle(odev);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<List<OdevTopluAtamaDTO>> TopluAtamalariListeleAsync()
    {
        var depo = _unitOfWork.RepositoryGetir<OdevTopluAtama>();

        var topluAtamalar = await depo.Sorgu
            .Include(t => t.Sinif)
            .Include(t => t.Odevler)
            .OrderByDescending(t => t.VerilisTarihi)
            .ToListAsync();

        return topluAtamalar.Select(t => t.Dto()).ToList();
    }

    public async Task<IslemSonucu<OdevTopluAtamaDTO>> TopluAtamaEkleAsync(OdevTopluAtamaKaydetDTO dto)
    {
        var dogrulamaSonucu = await _topluAtamaDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OdevTopluAtamaDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var sinifOgrencileri = await ogrenciDepo.ListeleAsync(o => o.SinifId == dto.SinifId && o.AktifMi);
        if (sinifOgrencileri.Count == 0)
        {
            return IslemSonucu<OdevTopluAtamaDTO>.Basarisiz("Bu sınıfta aktif öğrenci olmadığı için toplu ödev verilemedi.");
        }

        var topluAtamaDepo = _unitOfWork.RepositoryGetir<OdevTopluAtama>();
        var topluAtama = dto.Entity();
        await topluAtamaDepo.EkleAsync(topluAtama);
        await _unitOfWork.KaydetAsync();

        // sınıftaki her aktif öğrenci için ayrı bir odev kaydı açıyoruz, hepsi bu toplu atamaya bağlanıyor
        var odevDepo = _unitOfWork.RepositoryGetir<Odev>();
        foreach (var ogrenci in sinifOgrencileri)
        {
            await odevDepo.EkleAsync(new Odev
            {
                OgrenciId = ogrenci.Id,
                DersId = dto.DersId,
                KonuBasligi = dto.Baslik,
                VerilisTarihi = dto.VerilisTarihi,
                SonTeslimTarihi = dto.SonTeslimTarihi,
                Durum = OdevDurumu.Verildi,
                TopluAtamaId = topluAtama.Id
            });
        }

        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OdevTopluAtamaDTO>.Basar(topluAtama.Dto());
    }

    public async Task<IslemSonucu> TopluAtamaSilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<OdevTopluAtama>();
        var topluAtama = await depo.TekGetirAsync(id);
        if (topluAtama is null)
        {
            return IslemSonucu.Basarisiz("Silinecek toplu atama bulunamadı.");
        }

        depo.SilmeyeIsaretle(topluAtama);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    public async Task<OgrenciOdevOzetiDTO> OgrenciOzetiHesaplaAsync(int ogrenciId)
    {
        var odevler = await ListeleAsync(new OdevFiltreDTO { OgrenciId = ogrenciId });

        var toplam = odevler.Count;
        var tamamlanan = odevler.Count(o => o.Durum == OdevDurumu.Tamamlandi);
        var geciken = odevler.Count(o => o.Durum == OdevDurumu.Gecikti);

        return new OgrenciOdevOzetiDTO
        {
            ToplamOdevSayisi = toplam,
            TamamlananOdevSayisi = tamamlanan,
            GecikenOdevSayisi = geciken,
            TamamlamaOrani = YuzdeHesaplayici.Hesapla(tamamlanan, toplam)
        };
    }

    public async Task<List<OdevDTO>> YaklasanTeslimTarihleriniListeleAsync(int adet)
    {
        var odevler = await ListeleAsync(new OdevFiltreDTO { Durum = OdevDurumu.Verildi });

        return odevler
            .Where(o => o.SonTeslimTarihi.Date >= DateTime.Today)
            .OrderBy(o => o.SonTeslimTarihi)
            .Take(adet)
            .ToList();
    }

    public async Task<int> HaftalikTamamlamaOraniHesaplaAsync()
    {
        var haftaBaslangici = HaftaYardimcisi.HaftaBaslangici(DateTime.Today);
        var haftaBitisi = HaftaYardimcisi.HaftaBitisi(haftaBaslangici);

        var odevler = await ListeleAsync();
        var buHaftakiler = odevler
            .Where(o => o.SonTeslimTarihi.Date >= haftaBaslangici && o.SonTeslimTarihi.Date < haftaBitisi)
            .ToList();

        if (buHaftakiler.Count == 0)
        {
            return 0;
        }

        var tamamlanan = buHaftakiler.Count(o => o.Durum == OdevDurumu.Tamamlandi);
        return YuzdeHesaplayici.Hesapla(tamamlanan, buHaftakiler.Count);
    }
}
