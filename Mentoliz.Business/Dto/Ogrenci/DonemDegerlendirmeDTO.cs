using Mentoliz.Business.Dto.Deneme;
using Mentoliz.Business.Dto.Hedef;

namespace Mentoliz.Business.Dto.Ogrenci;

// dönem sonu raporunda kullanılıyor, tek denemeye değil bir tarih aralığının tamamına bakıyor
public class DonemDegerlendirmeDTO
{
    public DateTime DonemBaslangic { get; set; }

    public DateTime DonemBitis { get; set; }

    public int DenemeSayisi { get; set; }

    public decimal? DonemIlkToplamNet { get; set; }

    public decimal? DonemSonToplamNet { get; set; }

    public List<DersDegisimDTO> DersKarsilastirmalari { get; set; } = [];

    public int OdevSayisi { get; set; }

    public int OdevTamamlamaOrani { get; set; }

    public int GorusmeSayisi { get; set; }

    public string? SonAlinanKarar { get; set; }

    public HedefKarsilastirmaDTO? HedefKarsilastirma { get; set; }

    public string OtomatikYorum { get; set; } = string.Empty;
}
