using Mentoliz.Business.Dto.Odev;
using Mentoliz.Entities;
using Mentoliz.Entities.Enums;

namespace Mentoliz.Business.Mapping;

public static class OdevEslemeleri
{
    public static OdevDTO Dto(this Odev odev) => new()
    {
        Id = odev.Id,
        OgrenciId = odev.OgrenciId,
        OgrenciAdSoyad = odev.Ogrenci is null ? string.Empty : $"{odev.Ogrenci.Ad} {odev.Ogrenci.Soyad}",
        DersId = odev.DersId,
        DersAdi = odev.Ders?.Ad ?? string.Empty,
        KonuBasligi = odev.KonuBasligi,
        KaynakAdi = odev.KaynakAdi,
        ToplamSoruSayisi = odev.ToplamSoruSayisi,
        TamamlananSoruSayisi = odev.TamamlananSoruSayisi,
        VerilisTarihi = odev.VerilisTarihi,
        SonTeslimTarihi = odev.SonTeslimTarihi,
        YapilmaTarihi = odev.YapilmaTarihi,
        Durum = odev.EfektifDurum(),
        Aciklama = odev.Aciklama,
        TopluAtamaId = odev.TopluAtamaId
    };

    // ödev hâlâ verildi durumundaysa ve teslim tarihi geçtiyse, veritabanına dokunmadan gecikti gösteriyoruz
    private static OdevDurumu EfektifDurum(this Odev odev)
    {
        if (odev.Durum == OdevDurumu.Verildi && odev.YapilmaTarihi is null && odev.SonTeslimTarihi.Date < DateTime.Today)
        {
            return OdevDurumu.Gecikti;
        }

        return odev.Durum;
    }

    public static OdevKaydetDTO KaydetDto(this Odev odev) => new()
    {
        Id = odev.Id,
        OgrenciId = odev.OgrenciId,
        DersId = odev.DersId,
        KonuBasligi = odev.KonuBasligi,
        KaynakAdi = odev.KaynakAdi,
        ToplamSoruSayisi = odev.ToplamSoruSayisi,
        TamamlananSoruSayisi = odev.TamamlananSoruSayisi,
        VerilisTarihi = odev.VerilisTarihi,
        SonTeslimTarihi = odev.SonTeslimTarihi,
        YapilmaTarihi = odev.YapilmaTarihi,
        Durum = odev.Durum,
        Aciklama = odev.Aciklama
    };

    public static Odev Entity(this OdevKaydetDTO dto) => new()
    {
        OgrenciId = dto.OgrenciId,
        DersId = dto.DersId,
        KonuBasligi = dto.KonuBasligi,
        KaynakAdi = dto.KaynakAdi,
        ToplamSoruSayisi = dto.ToplamSoruSayisi,
        TamamlananSoruSayisi = dto.TamamlananSoruSayisi,
        VerilisTarihi = dto.VerilisTarihi,
        SonTeslimTarihi = dto.SonTeslimTarihi,
        YapilmaTarihi = dto.YapilmaTarihi,
        Durum = dto.Durum,
        Aciklama = dto.Aciklama
    };

    public static void Uygula(this OdevKaydetDTO dto, Odev odev)
    {
        odev.OgrenciId = dto.OgrenciId;
        odev.DersId = dto.DersId;
        odev.KonuBasligi = dto.KonuBasligi;
        odev.KaynakAdi = dto.KaynakAdi;
        odev.ToplamSoruSayisi = dto.ToplamSoruSayisi;
        odev.TamamlananSoruSayisi = dto.TamamlananSoruSayisi;
        odev.VerilisTarihi = dto.VerilisTarihi;
        odev.SonTeslimTarihi = dto.SonTeslimTarihi;
        odev.YapilmaTarihi = dto.YapilmaTarihi;
        odev.Durum = dto.Durum;
        odev.Aciklama = dto.Aciklama;
    }

    public static OdevTopluAtamaDTO Dto(this OdevTopluAtama topluAtama) => new()
    {
        Id = topluAtama.Id,
        SinifId = topluAtama.SinifId,
        SinifAdi = topluAtama.Sinif?.Ad ?? string.Empty,
        DersId = topluAtama.DersId,
        DersAdi = topluAtama.Ders?.Ad ?? string.Empty,
        Baslik = topluAtama.Baslik,
        VerilisTarihi = topluAtama.VerilisTarihi,
        SonTeslimTarihi = topluAtama.SonTeslimTarihi,
        OdevSayisi = topluAtama.Odevler.Count
    };

    public static OdevTopluAtama Entity(this OdevTopluAtamaKaydetDTO dto) => new()
    {
        SinifId = dto.SinifId,
        DersId = dto.DersId,
        Baslik = dto.Baslik,
        VerilisTarihi = dto.VerilisTarihi,
        SonTeslimTarihi = dto.SonTeslimTarihi
    };

    public static void Uygula(this OdevTopluAtamaKaydetDTO dto, OdevTopluAtama topluAtama)
    {
        topluAtama.SinifId = dto.SinifId;
        topluAtama.DersId = dto.DersId;
        topluAtama.Baslik = dto.Baslik;
        topluAtama.VerilisTarihi = dto.VerilisTarihi;
        topluAtama.SonTeslimTarihi = dto.SonTeslimTarihi;
    }
}
