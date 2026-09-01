namespace Mentoliz.Business.Dto.Ayar;

public class YedekDosyaDTO
{
    public string DosyaYolu { get; set; } = string.Empty;

    public string DosyaAdi { get; set; } = string.Empty;

    public DateTime OlusturmaTarihi { get; set; }

    public long BoyutBayt { get; set; }
}
