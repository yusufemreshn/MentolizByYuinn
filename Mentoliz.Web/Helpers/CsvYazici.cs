using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Mentoliz.Web.Helpers;

// utf-8 bom'lu csv üretimi tek yerden yapılsın diye bütün dışa aktarma ekranları burayı kullanıyor
public static class CsvYazici
{
    private const string IcerikTuru = "text/csv";

    // excel türkçe yerel ayarda virgülü ondalık ayracı olarak kullanıyor, o yüzden sütun ayracı olarak noktalı virgül seçtik
    private const char Ayirici = ';';

    // controller'lar csv içeriğini kendileri üretip File() çağırmak yerine doğrudan bu indirilebilir sonucu dönüyor
    public static FileContentResult OlusturDosya(IEnumerable<string> basliklar, IEnumerable<IEnumerable<string?>> satirlar, string dosyaAdi)
    {
        var icerik = OlusturCsvIcerik(basliklar, satirlar);
        return new FileContentResult(icerik, IcerikTuru) { FileDownloadName = dosyaAdi };
    }

    public static byte[] OlusturCsvIcerik(IEnumerable<string> basliklar, IEnumerable<IEnumerable<string?>> satirlar)
    {
        var metin = new StringBuilder();
        metin.AppendLine(SatirOlustur(basliklar));

        foreach (var satir in satirlar)
        {
            metin.AppendLine(SatirOlustur(satir));
        }

        var govde = Encoding.UTF8.GetBytes(metin.ToString());
        var bom = Encoding.UTF8.GetPreamble();

        return [.. bom, .. govde];
    }

    private static string SatirOlustur(IEnumerable<string?> alanlar) =>
        string.Join(Ayirici, alanlar.Select(AlanKacisla));

    // alanda ayraç, tırnak veya satır sonu varsa tırnak içine alıp iç tırnakları ikiye katlıyoruz
    private static string AlanKacisla(string? alan)
    {
        alan ??= string.Empty;

        if (alan.Contains(Ayirici) || alan.Contains('"') || alan.Contains('\n') || alan.Contains('\r'))
        {
            return $"\"{alan.Replace("\"", "\"\"")}\"";
        }

        return alan;
    }
}
