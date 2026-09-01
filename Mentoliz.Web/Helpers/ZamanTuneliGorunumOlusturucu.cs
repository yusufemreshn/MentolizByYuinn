namespace Mentoliz.Web.Helpers;

// zaman tüneli ögesinin tür alanına göre ikon ve etiket seçiyor, aynı switch her yerde tekrar yazılmasın diye
public static class ZamanTuneliGorunumOlusturucu
{
    public static string Ikon(string tur) => tur switch
    {
        "gorusme" => "bi-chat-dots",
        "odev" => "bi-journal-text",
        "deneme" => "bi-clipboard-data",
        "gorev" => "bi-check2-square",
        _ => "bi-dot"
    };

    public static string Etiket(string tur) => tur switch
    {
        "gorusme" => "Görüşme",
        "odev" => "Ödev",
        "deneme" => "Deneme",
        "gorev" => "Görev",
        _ => "Kayıt"
    };
}
