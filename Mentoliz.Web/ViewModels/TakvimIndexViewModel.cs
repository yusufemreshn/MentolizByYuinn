using Mentoliz.Business.Dto.Ortak;

namespace Mentoliz.Web.ViewModels;

public class TakvimIndexViewModel
{
    // gösterilen ayın ilk günü, ay başlığı ve önceki/sonraki ay linkleri bundan türetiliyor
    public DateTime AyBaslangici { get; set; }

    public List<TakvimOgesiDTO> Ogeler { get; set; } = [];
}
