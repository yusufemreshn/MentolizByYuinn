using Mentoliz.Business.Dto.Ogrenci;

namespace Mentoliz.Web.Helpers;

// risk puanının dört alt bileşeninden en yüksek olanını kısa bir türkçe etikete çeviriyor, ana sayfada tek satırlık sebep göstermek için
public static class RiskAciklamaOlusturucu
{
    public static string BaskinSebep(OgrenciRiskDTO risk)
    {
        var enYuksek = new[] { risk.NetTrendPuani, risk.OdevPuani, risk.GorusmePuani, risk.HedefPuani }.Max();

        if (enYuksek == 0)
        {
            return "birden fazla küçük etken";
        }

        // eşitlik durumunda hep aynı sırayla kontrol ediyoruz, net düşüşü en somut sinyal olduğu için önce o
        if (risk.NetTrendPuani == enYuksek)
        {
            return "deneme netinde düşüş";
        }

        if (risk.OdevPuani == enYuksek)
        {
            return "ödev aksaması";
        }

        if (risk.GorusmePuani == enYuksek)
        {
            return "uzun süredir görüşülmedi";
        }

        return "hedeften geride";
    }
}
