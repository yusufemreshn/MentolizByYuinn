namespace Mentoliz.Business.Dto.Deneme;

// önceki denemeden doldurma düğmesi için, öğrenci id'siyle eşlenmiş önceki deneme detayları
public class OgrenciOncekiSonucDTO
{
    public int OgrenciId { get; set; }

    public List<DenemeSonucDetayKaydetDTO> Detaylar { get; set; } = [];
}
