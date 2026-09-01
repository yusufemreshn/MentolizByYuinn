namespace Mentoliz.Entities.Enums;

// öğrencinin sınıf programı üzerine eklenen veya ondan çıkarılan özel durumlar
public enum ProgramIstisnaTuru
{
    OzelDers = 1,
    Etut = 2,
    Telafi = 3,
    // sınıfın normal dersi bu öğrenci için düşülüyor demek
    Iptal = 4
}
