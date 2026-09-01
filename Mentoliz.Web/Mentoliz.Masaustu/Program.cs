namespace Mentoliz.Masaustu;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // webview2 kendi içinde per monitor v2 modunda çalışıyor, pencere sistemaware kalınca ikisi arasındaki dpi uyuşmazlığı başlık çubuğundaki kapat büyüt küçült düğmelerini tıklanamaz hale getiriyordu
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Application.Run(new AnaPencere());
    }
}
