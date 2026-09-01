namespace Mentoliz.Business.Helpers;

// tamamlama oranı gibi yüzde hesapları birkaç serviste tekrar ediyordu, tek yerden yapılsın diye burada topladık
public static class YuzdeHesaplayici
{
    private const decimal YuzdeCarpani = 100m;

    // toplam sıfırsa bölme hatası yerine sıfır dönüyor
    public static int Hesapla(int kismi, int toplam) =>
        toplam == 0 ? 0 : (int)Math.Round(kismi * YuzdeCarpani / toplam);
}
