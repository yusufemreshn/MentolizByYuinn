namespace Mentoliz.Web.Helpers;

// tek bir üçüncü taraf kütüphane bildirimini temsil ediyor
public record UcuncuTarafBildirimi(string Kutuphane, string Surum, string Lisans, string Baglanti);

// ayarlar sayfasındaki hakkında bölümünde gösterilen liste, THIRD-PARTY-NOTICES.md ile birebir aynı tutulmalı
// yeni bir paket eklendiğinde THIRD-PARTY-NOTICES.md'nin yanında bu liste de güncellenecek
public static class UcuncuTarafBildirimleri
{
    public static readonly IReadOnlyList<UcuncuTarafBildirimi> Tumu =
    [
        new("Bootstrap", "5.3.3", "MIT", "https://getbootstrap.com/"),
        new("Microsoft.EntityFrameworkCore.Sqlite", "10.0.11", "MIT", "https://github.com/dotnet/efcore"),
        new("Microsoft.EntityFrameworkCore.Design", "10.0.11", "MIT", "https://github.com/dotnet/efcore"),
        new("FluentValidation", "12.1.1", "Apache-2.0", "https://github.com/FluentValidation/FluentValidation"),
        new("FluentValidation.DependencyInjectionExtensions", "12.1.1", "Apache-2.0", "https://github.com/FluentValidation/FluentValidation"),
        new("Bootstrap Icons", "1.11.3", "MIT", "https://icons.getbootstrap.com/"),
        new("Inter", "4.1", "SIL Open Font License 1.1", "https://rsms.me/inter/"),
        new("Chart.js", "4.4.7", "MIT", "https://www.chartjs.org/")
    ];
}
