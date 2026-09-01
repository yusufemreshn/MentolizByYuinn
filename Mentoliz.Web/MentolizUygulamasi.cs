using Mentoliz.Business;
using Mentoliz.Web.Filters;

namespace Mentoliz.Web;

// asp.net core sunucusunun kurulumunu buraya topladık çünkü hem taraycıdan çalıştırdığımız program.cs hem de masaüstü kabuğu aynı sunucuyu kurmak zorunda
public static class MentolizUygulamasi
{
    // port verilirse sunucu sadece o adreste dinliyor, masaüstü kabuğu kendi yerel portunu buradan veriyor
    public static WebApplication Olustur(string[] args, int? port = null)
    {
        // içerik kök dizinini uygulamanın kendi klasörüne sabitliyoruz, masaüstü kabuğu başka bir çalışma dizininden başlatılsa bile appsettings.json bulunsun diye
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        // aktif temayı her view'a otomatik taşıyan filtre burada global olarak ekleniyor
        // masaüstü kabuğu çalıştırdığında asıl çalışan derleme kendisi olduğu için controller ve view'ları burada elle ekliyoruz, yoksa hiçbiri bulunamıyor
        builder.Services.AddControllersWithViews(secenekler => secenekler.Filters.Add<AktifTemaSonucFiltresi>())
            .AddApplicationPart(typeof(MentolizUygulamasi).Assembly);

        // deneme sonuç girişindeki satır bazlı otomatik kayıt json gövdeyle istek atıyor, form alanı olmadığı için doğrulama token'ını başlıktan okuyoruz
        builder.Services.AddAntiforgery(secenekler => secenekler.HeaderName = "X-CSRF-TOKEN");

        // veritabanı ve iş katmanı kayıtları business üzerinden yapılıyor, web projesi dataaccess'i hiç tanımıyor
        var baglantiDizesi = builder.Configuration.GetConnectionString("MentolizVeriTabani")
            ?? throw new InvalidOperationException("appsettings.json içinde MentolizVeriTabani bağlantı dizesi bulunamadı.");
        baglantiDizesi = BaglantiDizesiniMutlakYolaCevir(baglantiDizesi, builder.Environment.ContentRootPath);
        builder.Services.AddMentolizIsKatmani(baglantiDizesi);

        if (port.HasValue)
        {
            // masaüstü kabuğu sunucuyu kendi yerel portunda açıyor, dışarıdan erişilebilir olmasına gerek yok
            builder.WebHost.UseUrls($"http://127.0.0.1:{port.Value}");
        }

        var app = builder.Build();

        // veritabanı dosyası ilk açılışta yoksa oluşturuluyor, bekleyen migration varsa uygulanıyor
        app.Services.VeriTabaniniHazirla();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // varsayılan hsts süresi otuz gün, üretimde bu süre farklı ayarlanabilir
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        // yeni statik dosya sistemi masaüstü kabuğunda dosyaları bulamıyordu, klasik yöntem her iki taraftan da çalıştırılınca sorunsuz çalışıyor
        app.UseStaticFiles();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }

    // veritabanı yolu appsettings.json içinde göreli yazıyor, uygulama hangi klasörden çalıştırılırsa çalıştırılsın her zaman kendi klasöründeki app_data'yı kullansın diye mutlak yola çeviriyoruz
    private static string BaglantiDizesiniMutlakYolaCevir(string baglantiDizesi, string icerikKokDizini)
    {
        const string veriKaynagiOnEki = "Data Source=";

        if (!baglantiDizesi.StartsWith(veriKaynagiOnEki, StringComparison.OrdinalIgnoreCase))
        {
            return baglantiDizesi;
        }

        var dosyaYolu = baglantiDizesi[veriKaynagiOnEki.Length..];
        if (Path.IsPathRooted(dosyaYolu))
        {
            return baglantiDizesi;
        }

        var mutlakYol = Path.GetFullPath(Path.Combine(icerikKokDizini, dosyaYolu));
        return veriKaynagiOnEki + mutlakYol;
    }
}
