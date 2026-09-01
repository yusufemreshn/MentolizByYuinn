using Mentoliz.Business.Dto.Odev;

namespace Mentoliz.Web.ViewModels;

// tek bir toplu ödev atamasına bağlı öğrenci listesini durum sütunuyla birlikte gösteren yoklama ızgarası
public class OdevYoklamaViewModel
{
    public List<OdevTopluAtamaDTO> TopluAtamalar { get; set; } = [];

    public int? SeciliTopluAtamaId { get; set; }

    public List<OdevDTO> Odevler { get; set; } = [];
}
