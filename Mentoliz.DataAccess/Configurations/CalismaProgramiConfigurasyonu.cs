using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class CalismaProgramiConfigurasyonu : TemelEntityKonfigurasyonu<CalismaProgrami>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<CalismaProgrami> builder)
    {
        builder.Property(c => c.Aciklama).HasMaxLength(500);

        builder.HasOne(c => c.Ogrenci)
            .WithMany(o => o.CalismaProgramlari)
            .HasForeignKey(c => c.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
