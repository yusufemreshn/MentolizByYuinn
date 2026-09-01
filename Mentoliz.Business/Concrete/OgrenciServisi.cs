using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Business.Mapping;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.Business.Concrete;

public class OgrenciServisi : IOgrenciServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<OgrenciKaydetDTO> _dogrulayici;

    public OgrenciServisi(IUnitOfWork unitOfWork, IValidator<OgrenciKaydetDTO> dogrulayici)
    {
        _unitOfWork = unitOfWork;
        _dogrulayici = dogrulayici;
    }

    public async Task<List<OgrenciDTO>> ListeleAsync(OgrenciFiltreDTO? filtre = null)
    {
        var depo = _unitOfWork.RepositoryGetir<Ogrenci>();

        var sorgu = depo.Sorgu
            .Include(o => o.Sinif)
            .Include(o => o.Veliler)
            .AsQueryable();

        if (filtre is not null)
        {
            if (!string.IsNullOrWhiteSpace(filtre.Arama))
            {
                // ef core sqlite'ta contains'i instr() ile çeviriyor ve o büyük küçük harfe duyarlı, like kullanınca aramaya "ada" yazınca "Ada" da bulunuyor
                var arama = $"%{filtre.Arama.Trim()}%";
                sorgu = sorgu.Where(o =>
                    EF.Functions.Like(o.Ad, arama) ||
                    EF.Functions.Like(o.Soyad, arama) ||
                    (o.OgrenciNo != null && EF.Functions.Like(o.OgrenciNo, arama)));
            }

            if (filtre.SinifId.HasValue)
            {
                sorgu = sorgu.Where(o => o.SinifId == filtre.SinifId.Value);
            }

            if (filtre.AktifMi.HasValue)
            {
                sorgu = sorgu.Where(o => o.AktifMi == filtre.AktifMi.Value);
            }
        }

        var ogrenciler = await sorgu.OrderBy(o => o.Ad).ThenBy(o => o.Soyad).ToListAsync();

        return ogrenciler.Select(o => o.Dto()).ToList();
    }

    public async Task<OgrenciKaydetDTO?> DuzenlemeIcinGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Ogrenci>();

        var ogrenci = await depo.Sorgu
            .Include(o => o.Veliler)
            .FirstOrDefaultAsync(o => o.Id == id);

        return ogrenci?.KaydetDto();
    }

    public async Task<OgrenciDTO?> TekGetirAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Ogrenci>();

        var ogrenci = await depo.Sorgu
            .Include(o => o.Sinif)
            .Include(o => o.Veliler)
            .FirstOrDefaultAsync(o => o.Id == id);

        return ogrenci?.Dto();
    }

    public async Task<IslemSonucu<OgrenciDTO>> EkleAsync(OgrenciKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();

        if (await OgrenciNoKullanimdaMiAsync(ogrenciDepo, dto.OgrenciNo, halihazirdakiId: null))
        {
            return IslemSonucu<OgrenciDTO>.Basarisiz("Bu öğrenci numarası zaten kullanılıyor.");
        }

        var ogrenci = dto.Entity();
        foreach (var veliDto in dto.Veliler)
        {
            // navigation üzerinden eklenince ilişki kaydedilirken ogrenciid otomatik set ediliyor
            ogrenci.Veliler.Add(veliDto.Entity(0));
        }

        await ogrenciDepo.EkleAsync(ogrenci);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciDTO>.Basar(ogrenci.Dto());
    }

    public async Task<IslemSonucu<OgrenciDTO>> GuncelleAsync(OgrenciKaydetDTO dto)
    {
        var dogrulamaSonucu = await _dogrulayici.ValidateAsync(dto);
        if (!dogrulamaSonucu.IsValid)
        {
            return IslemSonucu<OgrenciDTO>.Basarisiz(dogrulamaSonucu.Errors.Select(h => h.ErrorMessage));
        }

        var ogrenciDepo = _unitOfWork.RepositoryGetir<Ogrenci>();

        var mevcutOgrenci = await ogrenciDepo.Sorgu
            .Include(o => o.Veliler)
            .FirstOrDefaultAsync(o => o.Id == dto.Id);

        if (mevcutOgrenci is null)
        {
            return IslemSonucu<OgrenciDTO>.Basarisiz("Güncellenecek öğrenci bulunamadı.");
        }

        if (await OgrenciNoKullanimdaMiAsync(ogrenciDepo, dto.OgrenciNo, halihazirdakiId: dto.Id))
        {
            return IslemSonucu<OgrenciDTO>.Basarisiz("Bu öğrenci numarası zaten kullanılıyor.");
        }

        dto.Uygula(mevcutOgrenci);

        VelileriEslestir(dto.Veliler, mevcutOgrenci);

        await _unitOfWork.KaydetAsync();

        return IslemSonucu<OgrenciDTO>.Basar(mevcutOgrenci.Dto());
    }

    public async Task<IslemSonucu> SilAsync(int id)
    {
        var depo = _unitOfWork.RepositoryGetir<Ogrenci>();

        // öğrenciye bağlı bütün kayıtları da işaretleyeceğimiz için hepsini burada birlikte çekiyoruz
        var ogrenci = await depo.Sorgu
            .Include(o => o.Veliler)
            .Include(o => o.ProgramIstisnalari)
            .Include(o => o.Odevler)
            .Include(o => o.DenemeSonuclari).ThenInclude(sonuc => sonuc.Detaylar)
            .Include(o => o.Gorusmeler)
            .Include(o => o.Hedefler).ThenInclude(hedef => hedef.DersNetleri)
            .Include(o => o.CalismaProgramlari).ThenInclude(program => program.Satirlar)
            .Include(o => o.KonuTakipleri)
            .Include(o => o.Gorevler)
            .Include(o => o.Tercihler)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (ogrenci is null)
        {
            return IslemSonucu.Basarisiz("Silinecek öğrenci bulunamadı.");
        }

        IliskiliKayitlariKaskadSil(ogrenci);
        depo.SilmeyeIsaretle(ogrenci);
        await _unitOfWork.KaydetAsync();

        return IslemSonucu.Basar();
    }

    // öğrenci silinince ona bağlı kayıtlar yumuşak silinmeden kalırsa sahipsiz kalıyor, analiz ve listeleme sorguları bundan yanlış sonuç üretebiliyordu
    private void IliskiliKayitlariKaskadSil(Ogrenci ogrenci)
    {
        KaskadSil(ogrenci.Veliler);
        KaskadSil(ogrenci.ProgramIstisnalari);
        KaskadSil(ogrenci.Odevler);

        foreach (var sonuc in ogrenci.DenemeSonuclari)
        {
            KaskadSil(sonuc.Detaylar);
        }
        KaskadSil(ogrenci.DenemeSonuclari);

        KaskadSil(ogrenci.Gorusmeler);

        foreach (var hedef in ogrenci.Hedefler)
        {
            KaskadSil(hedef.DersNetleri);
        }
        KaskadSil(ogrenci.Hedefler);

        foreach (var program in ogrenci.CalismaProgramlari)
        {
            KaskadSil(program.Satirlar);
        }
        KaskadSil(ogrenci.CalismaProgramlari);

        KaskadSil(ogrenci.KonuTakipleri);
        KaskadSil(ogrenci.Gorevler);
        KaskadSil(ogrenci.Tercihler);
    }

    // aynı yumuşak silme döngüsünü her ilişkili tip için tekrar tekrar yazmamak için tek bir generic yardımcıda topladık
    private void KaskadSil<TIliskili>(IEnumerable<TIliskili> kayitlar) where TIliskili : BaseEntity
    {
        var depo = _unitOfWork.RepositoryGetir<TIliskili>();
        foreach (var kayit in kayitlar)
        {
            depo.SilmeyeIsaretle(kayit);
        }
    }

    private static async Task<bool> OgrenciNoKullanimdaMiAsync(IRepository<Ogrenci> depo, string? ogrenciNo, int? halihazirdakiId)
    {
        if (string.IsNullOrWhiteSpace(ogrenciNo))
        {
            return false;
        }

        return await depo.VarMiAsync(o => o.OgrenciNo == ogrenciNo && o.Id != halihazirdakiId);
    }

    // formdan gelen veli listesini mevcut veliler ile karşılaştırıp ekleme, güncelleme ve silme işlemlerini uyguluyor
    private void VelileriEslestir(List<VeliKaydetDTO> gelenVeliler, Ogrenci mevcutOgrenci)
    {
        var veliDepo = _unitOfWork.RepositoryGetir<Veli>();

        var gelenIdler = gelenVeliler.Where(v => v.Id != 0).Select(v => v.Id).ToHashSet();
        foreach (var kaldirilanVeli in mevcutOgrenci.Veliler.Where(v => !gelenIdler.Contains(v.Id)).ToList())
        {
            veliDepo.SilmeyeIsaretle(kaldirilanVeli);
        }

        foreach (var veliDto in gelenVeliler)
        {
            if (veliDto.Id == 0)
            {
                mevcutOgrenci.Veliler.Add(veliDto.Entity(mevcutOgrenci.Id));
                continue;
            }

            var mevcutVeli = mevcutOgrenci.Veliler.FirstOrDefault(v => v.Id == veliDto.Id);
            if (mevcutVeli is not null)
            {
                veliDto.Uygula(mevcutVeli);
            }
        }
    }
}
