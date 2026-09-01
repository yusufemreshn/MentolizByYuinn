namespace Mentoliz.Business.Helpers;

// servis metotları veri döndürmeden sadece başarı bilgisi verecekse bunu kullanıyor
public class IslemSonucu
{
    public bool Basarili { get; init; }

    // controller bu mesajları doğrudan kullanıcıya gösterecek, o yüzden hepsi türkçe ve anlaşılır olmalı
    public List<string> HataMesajlari { get; init; } = [];

    public static IslemSonucu Basar() => new() { Basarili = true };

    public static IslemSonucu Basarisiz(params string[] hatalar) => new() { Basarili = false, HataMesajlari = [.. hatalar] };

    public static IslemSonucu Basarisiz(IEnumerable<string> hatalar) => new() { Basarili = false, HataMesajlari = [.. hatalar] };
}

// servis metodu veri de döndürmesi gerektiğinde bu kullanılıyor, örneğin yeni eklenen kaydın kendisi
public class IslemSonucu<TVeri> : IslemSonucu
{
    public TVeri? Veri { get; init; }

    public static IslemSonucu<TVeri> Basar(TVeri veri) => new() { Basarili = true, Veri = veri };

    public static new IslemSonucu<TVeri> Basarisiz(params string[] hatalar) => new() { Basarili = false, HataMesajlari = [.. hatalar] };

    public static new IslemSonucu<TVeri> Basarisiz(IEnumerable<string> hatalar) => new() { Basarili = false, HataMesajlari = [.. hatalar] };
}
