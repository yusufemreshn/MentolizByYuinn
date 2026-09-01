using Mentoliz.Business.Dto.Ortak;

namespace Mentoliz.Business.Abstract;

public interface ITakvimServisi
{
    // verilen tarih aralığındaki görüşme, ödev teslim tarihi, deneme ve görev kayıtlarını tek listede birleştirir
    Task<List<TakvimOgesiDTO>> OgeleriGetirAsync(DateTime baslangic, DateTime bitisDahilDegil);
}
