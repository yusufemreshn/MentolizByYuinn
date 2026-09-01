using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OgrenciConfigurasyonu : TemelEntityKonfigurasyonu<Ogrenci>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Ogrenci> builder)
    {
        builder.Property(o => o.Ad).IsRequired().HasMaxLength(100);

        builder.Property(o => o.Soyad).IsRequired().HasMaxLength(100);

        builder.Property(o => o.Telefon).HasMaxLength(20);

        builder.Property(o => o.OgrenciNo).HasMaxLength(30);

        builder.Property(o => o.OkulAdi).HasMaxLength(200);

        builder.Property(o => o.FotografYolu).HasMaxLength(500);

        builder.Property(o => o.GenelNotlar).HasMaxLength(4000);

        // öğrenci numarası girilmemiş öğrenciler çok, bu yüzden sadece dolu ve silinmemiş kayıtlar arasında benzersizlik aranıyor
        // silinmemiş şartı da lazım yoksa yumuşak silinen bir öğrencinin numarası bir daha hiç kullanılamaz
        builder.HasIndex(o => o.OgrenciNo).IsUnique().HasFilter("OgrenciNo IS NOT NULL AND SilindiMi = 0");

        // sınıf silinirse öğrenci silinmesin, sadece sınıfsız kalsın
        builder.HasOne(o => o.Sinif)
            .WithMany(s => s.Ogrenciler)
            .HasForeignKey(o => o.SinifId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
