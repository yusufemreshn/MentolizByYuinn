using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Concrete;
using Mentoliz.DataAccess.Context;

namespace Mentoliz.Business;

// web katmanı veri erişim katmanını doğrudan tanımıyor, bütün kayıtlar buradan tek elden geçiyor
public static class IsKatmaniServisKayitlari
{
    public static IServiceCollection AddMentolizIsKatmani(this IServiceCollection services, string baglantiDizesi)
    {
        services.VeriErisimiKatmaniniEkle(baglantiDizesi);

        // bu assembly içindeki bütün fluentvalidation kurallarını tek satırda topluyor, statik sınıf tür parametresi olamadığı için işaret olarak bir servis sınıfı kullanıldı
        services.AddValidatorsFromAssemblyContaining<OgrenciServisi>();

        services.AddScoped<IOgrenciServisi, OgrenciServisi>();
        services.AddScoped<ISinifServisi, SinifServisi>();
        services.AddScoped<IDersServisi, DersServisi>();
        services.AddScoped<IDersProgramiServisi, DersProgramiServisi>();
        services.AddScoped<IOgrenciProgramServisi, OgrenciProgramServisi>();
        services.AddScoped<IOdevServisi, OdevServisi>();
        services.AddScoped<ISinavTuruServisi, SinavTuruServisi>();
        services.AddScoped<IDenemeServisi, DenemeServisi>();
        services.AddScoped<IDenemeAnaliziServisi, DenemeAnaliziServisi>();
        services.AddScoped<IRiskDegerlendirmeServisi, RiskDegerlendirmeServisi>();
        services.AddScoped<IVeliIletisimServisi, VeliIletisimServisi>();
        services.AddScoped<IZamanTuneliServisi, ZamanTuneliServisi>();
        services.AddScoped<IGorusmeHazirlikServisi, GorusmeHazirlikServisi>();
        services.AddScoped<ITakvimServisi, TakvimServisi>();
        services.AddScoped<IDonemDegerlendirmeServisi, DonemDegerlendirmeServisi>();
        services.AddScoped<IOgrenciTercihiServisi, OgrenciTercihiServisi>();
        services.AddScoped<IGorusmeServisi, GorusmeServisi>();
        services.AddScoped<IHedefServisi, HedefServisi>();
        services.AddScoped<ICalismaProgramiServisi, CalismaProgramiServisi>();
        services.AddScoped<IKonuServisi, KonuServisi>();
        services.AddScoped<IGorevServisi, GorevServisi>();
        services.AddScoped<INotServisi, NotServisi>();
        services.AddScoped<IAyarServisi, AyarServisi>();
        services.AddScoped<IYedeklemeServisi, YedeklemeServisi>();
        services.AddScoped<IDemoVeriServisi, DemoVeriServisi>();

        return services;
    }

    public static void VeriTabaniniHazirla(this IServiceProvider serviceProvider)
    {
        serviceProvider.VeriTabaniniOlustur();
    }
}
