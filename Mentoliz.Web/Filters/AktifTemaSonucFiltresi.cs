using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mentoliz.Business.Abstract;
using Mentoliz.Business.Helpers;
using Mentoliz.Web.Helpers;

namespace Mentoliz.Web.Filters;

// hangi sayfaya gidilirse gidilsin html etiketindeki data-theme ve data-mod değerleri burada belirleniyor, her controller ayrı ayrı uğraşmasın diye
public class AktifTemaSonucFiltresi : IAsyncResultFilter
{
    private readonly IAyarServisi _ayarServisi;

    public AktifTemaSonucFiltresi(IAyarServisi ayarServisi)
    {
        _ayarServisi = ayarServisi;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // sadece bir view basılıyorsa uğraşıyoruz, redirect gibi durumlarda gerek yok
        if (context.Result is ViewResult viewResult)
        {
            var kayitliTema = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.SeciliTema);
            viewResult.ViewData["AktifTema"] = TemaSecenekleri.GecerliMi(kayitliTema) ? kayitliTema! : TemaSecenekleri.Varsayilan;

            var kayitliKoyuMod = await _ayarServisi.DegerGetirAsync(AyarAnahtarlari.KoyuMod);
            viewResult.ViewData["KoyuMu"] = bool.TryParse(kayitliKoyuMod, out var koyuMu) && koyuMu;
        }

        await next();
    }
}
