using Mentoliz.Business.Dto.Ogrenci;
using Mentoliz.Entities;

namespace Mentoliz.Business.Mapping;

public static class OgrenciEslemeleri
{
    public static OgrenciDTO Dto(this Ogrenci ogrenci) => new()
    {
        Id = ogrenci.Id,
        Ad = ogrenci.Ad,
        Soyad = ogrenci.Soyad,
        Telefon = ogrenci.Telefon,
        OgrenciNo = ogrenci.OgrenciNo,
        SinifId = ogrenci.SinifId,
        // sınıf navigation'ı yüklenmemişse boş kalıyor, servis tarafında include edilmesi lazım
        SinifAdi = ogrenci.Sinif?.Ad,
        DogumTarihi = ogrenci.DogumTarihi,
        Cinsiyet = ogrenci.Cinsiyet,
        OkulAdi = ogrenci.OkulAdi,
        Alan = ogrenci.Alan,
        KayitTarihi = ogrenci.KayitTarihi,
        AktifMi = ogrenci.AktifMi,
        FotografYolu = ogrenci.FotografYolu,
        GenelNotlar = ogrenci.GenelNotlar,
        Veliler = ogrenci.Veliler.Select(v => v.Dto()).ToList()
    };

    public static Ogrenci Entity(this OgrenciKaydetDTO dto) => new()
    {
        Ad = dto.Ad,
        Soyad = dto.Soyad,
        Telefon = dto.Telefon,
        OgrenciNo = dto.OgrenciNo,
        SinifId = dto.SinifId,
        DogumTarihi = dto.DogumTarihi,
        Cinsiyet = dto.Cinsiyet,
        OkulAdi = dto.OkulAdi,
        Alan = dto.Alan,
        KayitTarihi = dto.KayitTarihi,
        AktifMi = dto.AktifMi,
        FotografYolu = dto.FotografYolu,
        GenelNotlar = dto.GenelNotlar
    };

    // düzenleme formunu doldururken kullanılıyor, OgrenciDTO'dan farklı olarak sadece formda düzenlenebilen alanları taşıyor
    public static OgrenciKaydetDTO KaydetDto(this Ogrenci ogrenci) => new()
    {
        Id = ogrenci.Id,
        Ad = ogrenci.Ad,
        Soyad = ogrenci.Soyad,
        Telefon = ogrenci.Telefon,
        OgrenciNo = ogrenci.OgrenciNo,
        SinifId = ogrenci.SinifId,
        DogumTarihi = ogrenci.DogumTarihi,
        Cinsiyet = ogrenci.Cinsiyet,
        OkulAdi = ogrenci.OkulAdi,
        Alan = ogrenci.Alan,
        KayitTarihi = ogrenci.KayitTarihi,
        AktifMi = ogrenci.AktifMi,
        FotografYolu = ogrenci.FotografYolu,
        GenelNotlar = ogrenci.GenelNotlar,
        Veliler = ogrenci.Veliler.Select(v => v.KaydetDto()).ToList()
    };

    // güncellemede izlenen entity üzerine yazıyoruz, id ve veliler ayrı yönetiliyor
    public static void Uygula(this OgrenciKaydetDTO dto, Ogrenci ogrenci)
    {
        ogrenci.Ad = dto.Ad;
        ogrenci.Soyad = dto.Soyad;
        ogrenci.Telefon = dto.Telefon;
        ogrenci.OgrenciNo = dto.OgrenciNo;
        ogrenci.SinifId = dto.SinifId;
        ogrenci.DogumTarihi = dto.DogumTarihi;
        ogrenci.Cinsiyet = dto.Cinsiyet;
        ogrenci.OkulAdi = dto.OkulAdi;
        ogrenci.Alan = dto.Alan;
        ogrenci.KayitTarihi = dto.KayitTarihi;
        ogrenci.AktifMi = dto.AktifMi;
        ogrenci.FotografYolu = dto.FotografYolu;
        ogrenci.GenelNotlar = dto.GenelNotlar;
    }

    public static VeliDTO Dto(this Veli veli) => new()
    {
        Id = veli.Id,
        OgrenciId = veli.OgrenciId,
        Yakinlik = veli.Yakinlik,
        Ad = veli.Ad,
        Soyad = veli.Soyad,
        Telefon = veli.Telefon,
        Meslek = veli.Meslek,
        Eposta = veli.Eposta,
        BirincilIletisimMi = veli.BirincilIletisimMi
    };

    public static VeliKaydetDTO KaydetDto(this Veli veli) => new()
    {
        Id = veli.Id,
        Yakinlik = veli.Yakinlik,
        Ad = veli.Ad,
        Soyad = veli.Soyad,
        Telefon = veli.Telefon,
        Meslek = veli.Meslek,
        Eposta = veli.Eposta,
        BirincilIletisimMi = veli.BirincilIletisimMi
    };

    public static Veli Entity(this VeliKaydetDTO dto, int ogrenciId) => new()
    {
        OgrenciId = ogrenciId,
        Yakinlik = dto.Yakinlik,
        Ad = dto.Ad,
        Soyad = dto.Soyad,
        Telefon = dto.Telefon,
        Meslek = dto.Meslek,
        Eposta = dto.Eposta,
        BirincilIletisimMi = dto.BirincilIletisimMi
    };

    public static void Uygula(this VeliKaydetDTO dto, Veli veli)
    {
        veli.Yakinlik = dto.Yakinlik;
        veli.Ad = dto.Ad;
        veli.Soyad = dto.Soyad;
        veli.Telefon = dto.Telefon;
        veli.Meslek = dto.Meslek;
        veli.Eposta = dto.Eposta;
        veli.BirincilIletisimMi = dto.BirincilIletisimMi;
    }
}
