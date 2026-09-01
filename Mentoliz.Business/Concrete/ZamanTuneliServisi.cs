using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Gorev;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities;

namespace Mentoliz.Business.Concrete;

public class ZamanTuneliServisi : IZamanTuneliServisi
{
    // notlar uzun olabiliyor, zaman tünelinde tek satıra sığsın diye bu uzunluktan sonra kırpılıyor
    private const int NotKirpmaUzunlugu = 90;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IOdevServisi _odevServisi;
    private readonly IGorevServisi _gorevServisi;

    public ZamanTuneliServisi(
        IUnitOfWork unitOfWork,
        IGorusmeServisi gorusmeServisi,
        IOdevServisi odevServisi,
        IGorevServisi gorevServisi)
    {
        _unitOfWork = unitOfWork;
        _gorusmeServisi = gorusmeServisi;
        _odevServisi = odevServisi;
        _gorevServisi = gorevServisi;
    }

    public async Task<List<ZamanTuneliOgesiDTO>> OlusturAsync(int ogrenciId, int adet)
    {
        var ogeler = new List<ZamanTuneliOgesiDTO>();

        var gorusmeler = await _gorusmeServisi.ListeleAsync(new GorusmeFiltreDTO { OgrenciId = ogrenciId });
        ogeler.AddRange(gorusmeler.Select(GorusmedenOgeUret));

        var odevler = await _odevServisi.ListeleAsync(new OdevFiltreDTO { OgrenciId = ogrenciId });
        ogeler.AddRange(odevler.Select(OdevdenOgeUret));

        var gorevler = await _gorevServisi.ListeleAsync(new GorevFiltreDTO { OgrenciId = ogrenciId });
        ogeler.AddRange(gorevler.Select(GorevdenOgeUret));

        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();
        var denemeSonuclari = await sonucDepo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi)
            .Include(s => s.Deneme)
            .Include(s => s.Detaylar)
            .ToListAsync();
        ogeler.AddRange(denemeSonuclari.Select(DenemeSonucundanOgeUret));

        return ogeler.OrderByDescending(o => o.Tarih).Take(adet).ToList();
    }

    // görüşme türünün türkçe karşılığı (öğrenci/veli/öğretmen görüşmesi gibi) yalnızca web katmanındaki enum uzantısında var, business katmanı ona erişemediği için burada sadece konu metni kullanılıyor
    private static ZamanTuneliOgesiDTO GorusmedenOgeUret(GorusmeDTO gorusme) => new()
    {
        Tarih = gorusme.Tarih,
        Tur = "gorusme",
        Baslik = gorusme.Konu,
        Aciklama = Kirp(gorusme.Notlar)
    };

    private static ZamanTuneliOgesiDTO OdevdenOgeUret(OdevDTO odev) => new()
    {
        Tarih = odev.VerilisTarihi,
        Tur = "odev",
        Baslik = $"Ödev verildi: {odev.DersAdi} — {odev.KonuBasligi}",
        Aciklama = $"Son teslim {odev.SonTeslimTarihi:dd.MM.yyyy}"
    };

    private static ZamanTuneliOgesiDTO GorevdenOgeUret(GorevDTO gorev) => new()
    {
        Tarih = gorev.Tarih,
        Tur = "gorev",
        Baslik = gorev.Baslik,
        Aciklama = gorev.TamamlandiMi ? "Tamamlandı" : "Açık"
    };

    private static ZamanTuneliOgesiDTO DenemeSonucundanOgeUret(DenemeSonuc sonuc)
    {
        var toplamNet = sonuc.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis));

        return new ZamanTuneliOgesiDTO
        {
            Tarih = sonuc.Deneme.Tarih,
            Tur = "deneme",
            Baslik = $"{sonuc.Deneme.Ad} — Net: {toplamNet:0.##}",
            Aciklama = null
        };
    }

    private static string? Kirp(string? metin)
    {
        if (string.IsNullOrWhiteSpace(metin))
        {
            return null;
        }

        return metin.Length <= NotKirpmaUzunlugu ? metin : metin[..NotKirpmaUzunlugu] + "…";
    }
}
