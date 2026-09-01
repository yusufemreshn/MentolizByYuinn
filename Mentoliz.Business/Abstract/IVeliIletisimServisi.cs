namespace Mentoliz.Business.Abstract;

public interface IVeliIletisimServisi
{
    // öğrencinin güncel deneme, ödev ve hedef durumunu birleştirip veliye gönderilebilecek hazır bir türkçe metin üretir
    Task<string> MesajOlusturAsync(int ogrenciId);
}
