namespace Mentoliz.Business.Helpers;

// net hesabı tüm projede tek yerden yapılsın diye burada topladık
public static class NetHesaplayici
{
    // dört yanlış bir doğruyu götürüyor, ösym'nin standart net hesabı
    public static decimal Hesapla(int dogru, int yanlis) => dogru - (yanlis / 4m);
}
