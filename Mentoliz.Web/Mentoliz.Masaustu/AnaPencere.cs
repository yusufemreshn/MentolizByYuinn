using Mentoliz.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Web.WebView2.WinForms;

namespace Mentoliz.Masaustu;

// tek pencereli kabuk, içi tamamen webview2 kontrolüyle dolu, asıl uygulama zaten web projesinde
public class AnaPencere : Form
{
    private const int YerelSunucuPortu = 5299;

    private readonly WebView2 _tarayiciKontrolu = new() { Dock = DockStyle.Fill };

    // sunucuyu biz başlattıysak burada tutuyoruz, pencere kapanırken düzgün durdurmak için
    private WebApplication? _sunucu;

    public AnaPencere()
    {
        Text = "Mentoliz";
        Width = 1280;
        Height = 800;
        StartPosition = FormStartPosition.CenterScreen;
        // exe'ye gömülü simgeyi kullanıyoruz, yoksa pencere winforms'un genel varsayılan simgesiyle açılıyordu
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

        Controls.Add(_tarayiciKontrolu);

        Load += AnaPencere_Load;
        FormClosing += AnaPencere_FormClosing;
    }

    private async void AnaPencere_Load(object? gonderen, EventArgs e)
    {
        var adres = await SunucuyuBaslatVeAdresiGetirAsync();

        await _tarayiciKontrolu.EnsureCoreWebView2Async();
        _tarayiciKontrolu.CoreWebView2.Navigate(adres);
    }

    // aynı anda ikinci pencere açılırsa port zaten dolu oluyor, o durumda yeni sunucu açmadan var olana bağlanıyoruz
    private async Task<string> SunucuyuBaslatVeAdresiGetirAsync()
    {
        var adres = $"http://127.0.0.1:{YerelSunucuPortu}";

        try
        {
            _sunucu = MentolizUygulamasi.Olustur([], YerelSunucuPortu);
            await _sunucu.StartAsync();
        }
        catch (IOException)
        {
            _sunucu = null;
        }

        return adres;
    }

    private void AnaPencere_FormClosing(object? gonderen, FormClosingEventArgs e)
    {
        // sunucuyu kapatmazsak pencere kapansa da arka planda çalışmaya devam eder
        // task.run içinde çalıştırmazsak sunucunun içindeki await'ler bu arayüz iş parçacığına dönmeye çalışıp kilitleniyordu, kapat düğmesi hiç tepki vermiyordu
        if (_sunucu is not null)
        {
            Task.Run(() => _sunucu.StopAsync()).GetAwaiter().GetResult();
        }
    }
}
