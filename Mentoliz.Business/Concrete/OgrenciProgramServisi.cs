using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Program;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class OgrenciProgramServisi : IOgrenciProgramServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<OgrenciProgramIstisnasiKaydetDTO> _istisnaDogrulayici;

    public OgrenciProgramServisi(
        IUnitOfWork unitOfWork,
        IValidator<OgrenciProgramIstisnasiKaydetDTO> istisnaDogrulayici)
    {
        _unitOfWork = unitOfWork;
        _istisnaDogrulayici = istisnaDogrulayici;
    }

    public async Task<List<OgrenciProgramIstisnasiDTO>> IstisnalariListeleAsync(int ogrenciId)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();

        // sqlite timespan alanını sıralamayı desteklemiyor, saat sıralamasını bellekte yapıyoruz
        var istisnalar = await depo.Sorgu
            .Where(i => i.OgrenciId == ogrenciId)
            .Include(i => i.Ders)
            .ToListAsync();

        return istisnalar
            .OrderBy(i => i.Gun).ThenBy(i => i.BaslangicSaati)
            .Select(i => i.Dto())
            .ToList();
    }

    public async Task<List<BirlesikProgramSatiriDTO>> BirlesikProgramiHesaplaAsync(int ogrenciId)
    {
        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();
        var ogrenci = await ogrenciDepo.TekGetirAsync(ogrenciId);
        if (ogrenci is null)
        {
            return [];
        }

        var satirlar = new List<BirlesikProgramSatiriDTO>();

        // önce öğrencinin bağlı olduğu sınıfın haftalık ders programı şablonunu ekliyoruz
        if (ogrenci.SinifId.HasValue)
        {
            var dersProgramiDepo = _unitOfWork.RepositoryGetir<DersProgramiSatiri>();
            var sinifSatirlari = await dersProgramiDepo.Sorgu
                .Where(s => s.SinifId == ogrenci.SinifId.Value)
                .Include(s => s.Ders)
                .ToListAsync();

            satirlar.AddRange(sinifSatirlari.Select(s => new BirlesikProgramSatiriDTO
            {
                Gun = s.Gun,
                BaslangicSaati = s.BaslangicSaati,
                BitisSaati = s.BitisSaati,
                DersAdi = s.Ders.Ad,
                OgretmenAdi = s.OgretmenAdi,
                DerslikAdi = s.DerslikAdi,
                Tur = null
            }));
        }

        // sadece bugün itibarıyla geçerli olan istisnaları dikkate alıyoruz, tarih aralığı geçmiş veya henüz başlamamışsa yok sayıyoruz
        var bugun = DateTime.Today;
        var istisnaDepo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();
        var istisnalar = await istisnaDepo.Sorgu
            .Where(i => i.OgrenciId == ogrenciId)
            .Include(i => i.Ders)
            .ToListAsync();

        var gecerliIstisnalar = istisnalar
            .Where(i => (!i.BaslangicTarihi.HasValue || i.BaslangicTarihi.Value.Date <= bugun) &&
                        (!i.BitisTarihi.HasValue || i.BitisTarihi.Value.Date >= bugun))
            .ToList();

        // iptal edilen saatler o saatteki sınıf dersiyle çakışıyorsa listeden düşürülüyor
        foreach (var iptal in gecerliIstisnalar.Where(i => i.Tur == ProgramIstisnaTuru.Iptal))
        {
            satirlar.RemoveAll(s =>
                s.Tur is null &&
                s.Gun == iptal.Gun &&
                s.BaslangicSaati < iptal.BitisSaati &&
                s.BitisSaati > iptal.BaslangicSaati);
        }

        // özel ders, etüt ve telafi saatleri birleşik programa ekleniyor
        satirlar.AddRange(gecerliIstisnalar
            .Where(i => i.Tur != ProgramIstisnaTuru.Iptal)
            .Select(i => new BirlesikProgramSatiriDTO
            {
                Gun = i.Gun,
                BaslangicSaati = i.BaslangicSaati,
                BitisSaati = i.BitisSaati,
                DersAdi = i.Ders?.Ad ?? string.Empty,
                OgretmenAdi = i.OgretmenAdi,
                Tur = i.Tur
            }));

        return satirlar.OrderBy(s => s.Gun).ThenBy(s => s.BaslangicSaati).ToList();
    }

    public async Task<List<HaftalikBulunmaSuresiDTO>> HaftalikBulunmaSuresiHesaplaAsync(int ogrenciId)
    {
        var birlesikProgram = await BirlesikProgramiHesaplaAsync(ogrenciId);

        return Enum.GetValues<Gun>()
            .Select(gun =>
            {
                var gununSatirlari = birlesikProgram.Where(s => s.Gun == gun).ToList();
                if (gununSatirlari.Count == 0)
                {
                    return new HaftalikBulunmaSuresiDTO { Gun = gun };
                }

                var giris = gununSatirlari.Min(s => s.BaslangicSaati);
                var cikis = gununSatirlari.Max(s => s.BitisSaati);

                return new HaftalikBulunmaSuresiDTO
                {
                    Gun = gun,
                    GirisSaati = giris,
                    CikisSaati = cikis,
                    ToplamSure = cikis - giris
                };
            })
            .ToList();
    }

    public async Task<OgrenciProgramIstisnasiKaydetDTO?> IstisnaDuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();
        var istisna = await depo.TekGetirAsync(id);

        return istisna?.KaydetDto();
    }

    public async Task<IslemSonucu<OgrenciProgramIstisnasiDTO>> IstisnaEkleAsync(OgrenciProgramIstisnasiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _istisnaDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciProgramIstisnasiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();
        var istisna = dto.Entity();

        await depo.EkleAsync(istisna);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciProgramIstisnasiDTO>.Basar(istisna.Dto());
    }

    public async Task<IslemSonucu<OgrenciProgramIstisnasiDTO>> IstisnaGuncelleAsync(OgrenciProgramIstisnasiKaydetDTO dto)
    {
        var dogrulamaSonucu = await _istisnaDogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciProgramIstisnasiDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var depo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();
        var mevcutIstisna = await depo.TekGetirAsync(dto.Id);
        if (mevcutIstisna is null)
        {
            return IslemSonucu<OgrenciProgramIstisnasiDTO>.Basarisiz("Güncellenecek program istisnası bulunamadı.");
        }

        dto.Uygula(mevcutIstisna);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciProgramIstisnasiDTO>.Basar(mevcutIstisna.Dto());
    }

    public async Task<IslemSonucu> IstisnaSilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<OgrenciProgramIstisnasi>();
        var istisna = await depo.TekGetirAsync(id);
        if (istisna is null)
        {
            return IslemSonucu.Basarisiz("Silinecek program istisnası bulunamadı.");
        }

        depo.SilmeyeIsaretle(istisna);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }
}
