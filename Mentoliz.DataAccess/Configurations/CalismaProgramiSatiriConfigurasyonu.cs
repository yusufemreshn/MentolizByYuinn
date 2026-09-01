using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class CalismaProgramiSatiriConfigurasyonu : TemelEntityKonfigurasyonu<CalismaProgramiSatiri>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<CalismaProgramiSatiri> builder)
    {
        builder.Property(c => c.Aciklama).HasMaxLength(500);

        builder.HasOne(c => c.CalismaProgrami)
            .WithMany(c => c.Satirlar)
            .HasForeignKey(c => c.CalismaProgramiId)
            .OnDelete(DeleteBehavior.Cascade);

        // serbest çalışma saati olabildiği için ders seçimi zorunlu değil, ders silinirse satır kalsın ama dersi boşa düşsün
        builder.HasOne(c => c.Ders)
            .WithMany(d => d.CalismaProgramiSatirlari)
            .HasForeignKey(c => c.DersId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
