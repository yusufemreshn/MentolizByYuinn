using Mentoliz.Business.Helpers;

namespace Mentoliz.Business.Abstract;

// bu servis geçici, arkadaşlara test amaçlı gönderirken hızlı örnek veri yüklemek/silmek için var
public interface IDemoVeriServisi
{
    Task<IslemSonucu> YukleAsync();

    Task<IslemSonucu> SilAsync();
}
