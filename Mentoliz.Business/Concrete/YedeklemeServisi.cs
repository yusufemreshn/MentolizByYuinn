using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.DataAccess.Context;
using Mentoliz.Business.Dto.Ayar;

namespace Mentoliz.Business.Concrete;

public class YedeklemeServisi : IYedeklemeServisi
{
    // yedek dosyalarını diğer db dosyalarından ayırt edip listelemek için önek kullanıyoruz
    private const string YedekDosyaOneki = "Mentoliz-";

    private readonly MentolizDbContext _baglam;

    public YedeklemeServisi(MentolizDbContext baglam)
    {
        _baglam = baglam;
    }

    public async Task<IslemSonucu<string>> YedekAlAsync(string hedefKlasor)
    {
        if (string.IsNullOrWhiteSpace(hedefKlasor))
        {
            return IslemSonucu<string>.Basarisiz("Yedekleme klasörü belirtilmedi.");
        }

        try
        {
            Directory.CreateDirectory(hedefKlasor);

            // wal modunda bekleyen değişiklikler ana dosyaya yazılmadan kopyalanırsa yedek eksik veri içerebilir
            await _baglam.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(TRUNCATE);");

            var kaynakDosya = VeriTabaniDosyaYolu();
            var hedefDosyaYolu = Path.Combine(hedefKlasor, $"{YedekDosyaOneki}{DateTime.Now:yyyyMMdd-HHmmss}.db");

            File.Copy(kaynakDosya, hedefDosyaYolu);

            return IslemSonucu<string>.Basar(hedefDosyaYolu);
        }
        catch (Exception hata)
        {
            return IslemSonucu<string>.Basarisiz($"Yedek alınamadı: {hata.Message}");
        }
    }

    public Task<List<YedekDosyaDTO>> YedekleriListeleAsync(string klasor)
    {
        if (string.IsNullOrWhiteSpace(klasor) || !Directory.Exists(klasor))
        {
            return Task.FromResult(new List<YedekDosyaDTO>());
        }

        var yedekler = Directory.GetFiles(klasor, $"{YedekDosyaOneki}*.db")
            .Select(dosyaYolu => new FileInfo(dosyaYolu))
            .OrderByDescending(bilgi => bilgi.CreationTime)
            .Select(bilgi => new YedekDosyaDTO
            {
                DosyaYolu = bilgi.FullName,
                DosyaAdi = bilgi.Name,
                OlusturmaTarihi = bilgi.CreationTime,
                BoyutBayt = bilgi.Length
            })
            .ToList();

        return Task.FromResult(yedekler);
    }

    public async Task<IslemSonucu> YedektenGeriYukleAsync(string yedekDosyaYolu)
    {
        if (!File.Exists(yedekDosyaYolu))
        {
            return IslemSonucu.Basarisiz("Belirtilen yedek dosyası bulunamadı.");
        }

        try
        {
            var kaynakDosya = VeriTabaniDosyaYolu();

            // canlı dosyayı açık tutan bağlantılar olabilir, kopyalamadan önce havuzu tamamen boşaltıyoruz
            await _baglam.Database.CloseConnectionAsync();
            SqliteConnection.ClearAllPools();

            File.Copy(yedekDosyaYolu, kaynakDosya, overwrite: true);

            // yedek dosyasının yanında eski wal/shm kalıntısı varsa karışıklık olmasın diye temizliyoruz
            DosyaVarsaSil($"{kaynakDosya}-wal");
            DosyaVarsaSil($"{kaynakDosya}-shm");

            return IslemSonucu.Basar();
        }
        catch (Exception hata)
        {
            return IslemSonucu.Basarisiz($"Yedekten geri yüklenemedi: {hata.Message}");
        }
    }

    private string VeriTabaniDosyaYolu()
    {
        var baglanti = _baglam.Database.GetDbConnection();
        return Path.GetFullPath(baglanti.DataSource);
    }

    private static void DosyaVarsaSil(string dosyaYolu)
    {
        if (File.Exists(dosyaYolu))
        {
            File.Delete(dosyaYolu);
        }
    }
}
