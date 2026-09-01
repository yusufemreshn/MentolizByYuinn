using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mentoliz.DataAccess.Repositories;

namespace Mentoliz.DataAccess.Context;

// bu katmanın dı kayıtları burada toplanıyor, web projesi bu sınıfı değil business katmanındaki sarmalayıcıyı çağıracak
public static class VeriErisimiServisKayitlari
{
    public static IServiceCollection VeriErisimiKatmaniniEkle(this IServiceCollection services, string baglantiDizesi)
    {
        services.AddDbContext<MentolizDbContext>(secenekler => secenekler.UseSqlite(baglantiDizesi));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    // uygulama açılışında veritabanı dosyası yoksa oluşturuluyor, bekleyen migration varsa uygulanıyor
    public static void VeriTabaniniOlustur(this IServiceProvider serviceProvider)
    {
        using var kapsam = serviceProvider.CreateScope();
        var context = kapsam.ServiceProvider.GetRequiredService<MentolizDbContext>();

        // sqlite dosyasının klasörü yoksa migrate hata veriyor, önce klasörün var olduğundan emin oluyoruz
        var veriKaynagi = context.Database.GetDbConnection().DataSource;
        var klasorYolu = Path.GetDirectoryName(veriKaynagi);
        if (!string.IsNullOrEmpty(klasorYolu))
        {
            Directory.CreateDirectory(klasorYolu);
        }

        context.Database.Migrate();
    }
}
