namespace Mentoliz.Business.Dto.Devamsizlik;

// öğrenci detay sayfasında ve risk hesaplamasında kullanılan son otuz günlük özet
public class OgrenciDevamsizlikOzetiDTO
{
    public int OgrenciId { get; set; }

    public int GelmemeSayisi { get; set; }

    public int MazeretliSayisi { get; set; }
}
