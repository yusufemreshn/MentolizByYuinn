namespace Mentoliz.Web.Helpers;

// ana sayfanın en üstündeki gündem cümlesini oluşturuyor, görüşme ve geciken ödev sayısı sıfırsa cümle ona göre değişiyor
public static class GundemOzetiOlusturucu
{
    public static string Cumle(int gorusmeSayisi, int gecikenOdevSayisi)
    {
        if (gorusmeSayisi == 0 && gecikenOdevSayisi == 0)
        {
            return "Bugün için planlanmış görüşme veya geciken ödev yok";
        }

        if (gorusmeSayisi == 0)
        {
            return $"Bugün görüşme yok ama {gecikenOdevSayisi} geciken ödev var";
        }

        if (gecikenOdevSayisi == 0)
        {
            return $"Bugün {gorusmeSayisi} görüşme var, geciken ödev yok";
        }

        return $"Bugün {gorusmeSayisi} görüşme, {gecikenOdevSayisi} geciken ödev var";
    }
}
