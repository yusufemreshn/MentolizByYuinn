using Mentoliz.Business.Helpers;

namespace Mentoliz.Business.Abstract;

// yedekleme klasörü, kurum adı gibi tekil ayarlar anahtar üzerinden okunup yazılıyor, id ile uğraşmaya gerek yok
public interface IAyarServisi
{
    Task<string?> DegerGetirAsync(string anahtar);

    // anahtar yoksa yeni kayıt açıyor, varsa üzerine yazıyor
    Task<IslemSonucu> DegerAtaAsync(string anahtar, string deger);
}
