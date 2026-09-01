using System.Text;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Repositories;
using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Gorusme;
using Mentoliz.Business.Dto.Odev;
using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Concrete;

public class DonemDegerlendirmeServisi : IDonemDegerlendirmeServisi
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOdevServisi _odevServisi;
    private readonly IGorusmeServisi _gorusmeServisi;
    private readonly IHedefServisi _hedefServisi;

    public DonemDegerlendirmeServisi(
        IUnitOfWork unitOfWork,
        IOdevServisi odevServisi,
        IGorusmeServisi gorusmeServisi,
        IHedefServisi hedefServisi)
    {
        _unitOfWork = unitOfWork;
        _odevServisi = odevServisi;
        _gorusmeServisi = gorusmeServisi;
        _hedefServisi = hedefServisi;
    }

    public async Task<DonemDegerlendirmeDTO> OlusturAsync(int ogrenciId, DateTime baslangic, DateTime bitis)
    {
        var dto = new DonemDegerlendirmeDTO
        {
            DonemBaslangic = baslangic,
            DonemBitis = bitis
        };

        await DenemeKarsilastirmasiEkleAsync(dto, ogrenciId, baslangic, bitis);
        await OdevOzetiEkleAsync(dto, ogrenciId, baslangic, bitis);
        await GorusmeOzetiEkleAsync(dto, ogrenciId, baslangic, bitis);

        dto.HedefKarsilastirma = await _hedefServisi.HedefeUzaklikHesaplaAsync(ogrenciId);

        dto.OtomatikYorum = YorumOlustur(dto);

        return dto;
    }

    // dönemin ilk ve son denemesindeki ortak dersleri karşılaştırıyoruz, aradaki tek tek denemelerle ilgilenmiyoruz çünkü rapor genel eğilimi gösteriyor
    private async Task<List<DenemeSonuc>> DonemSonuclariniGetirAsync(int ogrenciId, DateTime baslangic, DateTime bitis)
    {
        var sonucDepo = _unitOfWork.RepositoryGetir<DenemeSonuc>();

        var sonuclar = await sonucDepo.Sorgu
            .Where(s => s.OgrenciId == ogrenciId && !s.KatilmadiMi && s.Deneme.Tarih >= baslangic.Date && s.Deneme.Tarih <= bitis.Date)
            .Include(s => s.Deneme)
            .Include(s => s.Detaylar).ThenInclude(d => d.SinavTuruTest).ThenInclude(t => t.Ders)
            .OrderBy(s => s.Deneme.Tarih)
            .ToListAsync();

        return sonuclar;
    }

    private async Task DenemeKarsilastirmasiEkleAsync(DonemDegerlendirmeDTO dto, int ogrenciId, DateTime baslangic, DateTime bitis)
    {
        var sonuclar = await DonemSonuclariniGetirAsync(ogrenciId, baslangic, bitis);
        dto.DenemeSayisi = sonuclar.Count;

        if (sonuclar.Count == 0)
        {
            return;
        }

        var ilkSonuc = sonuclar.First();
        var sonSonuc = sonuclar.Last();

        dto.DonemIlkToplamNet = ilkSonuc.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis));
        dto.DonemSonToplamNet = sonSonuc.Detaylar.Sum(d => NetHesaplayici.Hesapla(d.Dogru, d.Yanlis));

        if (ilkSonuc.Id == sonSonuc.Id)
        {
            // dönemde tek deneme varsa karşılaştırma yapılamıyor, sadece o denemenin neti gösteriliyor
            return;
        }

        foreach (var sonrakiDetay in sonSonuc.Detaylar)
        {
            var oncekiDetay = ilkSonuc.Detaylar.FirstOrDefault(d => d.SinavTuruTest.DersId == sonrakiDetay.SinavTuruTest.DersId);
            if (oncekiDetay is null)
            {
                continue;
            }

            var oncekiNet = NetHesaplayici.Hesapla(oncekiDetay.Dogru, oncekiDetay.Yanlis);
            var sonrakiNet = NetHesaplayici.Hesapla(sonrakiDetay.Dogru, sonrakiDetay.Yanlis);

            dto.DersKarsilastirmalari.Add(new DersDegisimDTO
            {
                DersAdi = sonrakiDetay.SinavTuruTest.Ders.Ad,
                OncekiNet = oncekiNet,
                SonrakiNet = sonrakiNet,
                Degisim = sonrakiNet - oncekiNet
            });
        }

        dto.DersKarsilastirmalari = dto.DersKarsilastirmalari.OrderByDescending(d => d.Degisim).ToList();
    }

    private async Task OdevOzetiEkleAsync(DonemDegerlendirmeDTO dto, int ogrenciId, DateTime baslangic, DateTime bitis)
    {
        var odevler = await _odevServisi.ListeleAsync(new OdevFiltreDTO { OgrenciId = ogrenciId });
        var donemOdevleri = odevler
            .Where(o => o.SonTeslimTarihi.Date >= baslangic.Date && o.SonTeslimTarihi.Date <= bitis.Date)
            .ToList();

        dto.OdevSayisi = donemOdevleri.Count;
        dto.OdevTamamlamaOrani = YuzdeHesaplayici.Hesapla(
            donemOdevleri.Count(o => o.Durum == OdevDurumu.Tamamlandi),
            donemOdevleri.Count);
    }

    private async Task GorusmeOzetiEkleAsync(DonemDegerlendirmeDTO dto, int ogrenciId, DateTime baslangic, DateTime bitis)
    {
        var gorusmeler = await _gorusmeServisi.ListeleAsync(new GorusmeFiltreDTO { OgrenciId = ogrenciId });
        var donemGorusmeleri = gorusmeler
            .Where(g => g.Tarih.Date >= baslangic.Date && g.Tarih.Date <= bitis.Date)
            .OrderByDescending(g => g.Tarih)
            .ToList();

        dto.GorusmeSayisi = donemGorusmeleri.Count;
        dto.SonAlinanKarar = donemGorusmeleri.FirstOrDefault(g => !string.IsNullOrWhiteSpace(g.AlinanKarar))?.AlinanKarar;
    }

    // rapordaki sayıları düz metne çeviren bölüm, veli mesajındaki yorum üretme mantığıyla aynı yaklaşım
    private static string YorumOlustur(DonemDegerlendirmeDTO dto)
    {
        var metin = new StringBuilder();

        var gunSayisi = (dto.DonemBitis.Date - dto.DonemBaslangic.Date).Days;
        metin.Append($"{dto.DonemBaslangic:dd.MM.yyyy} ile {dto.DonemBitis:dd.MM.yyyy} arasındaki {gunSayisi} günlük dönemde ");

        if (dto.DenemeSayisi == 0)
        {
            metin.Append("hiç deneme sonucu girilmedi. ");
        }
        else if (dto.DenemeSayisi == 1)
        {
            metin.Append($"tek bir deneme sonucu girildi, toplam net {dto.DonemSonToplamNet:0.##} oldu. ");
        }
        else
        {
            var toplamDegisim = (dto.DonemSonToplamNet ?? 0) - (dto.DonemIlkToplamNet ?? 0);
            metin.Append($"{dto.DenemeSayisi} deneme sonucu girildi, toplam net {dto.DonemIlkToplamNet:0.##}'dan {dto.DonemSonToplamNet:0.##}'a ");
            metin.Append(toplamDegisim >= 0
                ? $"{toplamDegisim:0.##} net artış gösterdi. "
                : $"{Math.Abs(toplamDegisim):0.##} net düşüş gösterdi. ");

            var enCokYukselen = dto.DersKarsilastirmalari.Where(d => d.Degisim > 0).FirstOrDefault();
            var enCokDusen = dto.DersKarsilastirmalari.Where(d => d.Degisim < 0).LastOrDefault();
            if (enCokYukselen is not null)
            {
                metin.Append($"En çok gelişme {enCokYukselen.DersAdi} dersinde görüldü. ");
            }
            if (enCokDusen is not null)
            {
                metin.Append($"En çok gerileme {enCokDusen.DersAdi} dersinde görüldü. ");
            }
        }

        if (dto.OdevSayisi > 0)
        {
            metin.Append($"Bu dönem verilen {dto.OdevSayisi} ödevin %{dto.OdevTamamlamaOrani}'ini tamamladı. ");
        }

        metin.Append(dto.GorusmeSayisi == 0
            ? "Bu dönem içinde görüşme yapılmadı. "
            : $"Bu dönem içinde {dto.GorusmeSayisi} görüşme yapıldı");

        if (!string.IsNullOrWhiteSpace(dto.SonAlinanKarar))
        {
            metin.Append($", son alınan karar: {dto.SonAlinanKarar}. ");
        }
        else if (dto.GorusmeSayisi > 0)
        {
            metin.Append(". ");
        }

        if (dto.HedefKarsilastirma is not null && dto.HedefKarsilastirma.ToplamGuncelNet.HasValue)
        {
            var fark = dto.HedefKarsilastirma.ToplamGuncelNet.Value - dto.HedefKarsilastirma.ToplamHedefNet;
            metin.Append(fark >= 0
                ? "Hedeflenen toplam net şu an itibarıyla yakalanmış durumda."
                : $"Hedeflenen toplam nete göre şu an {Math.Abs(fark):0.##} net geride.");
        }

        return metin.ToString();
    }
}
