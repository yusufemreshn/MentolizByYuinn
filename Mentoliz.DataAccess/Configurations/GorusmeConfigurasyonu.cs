using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class GorusmeConfigurasyonu : TemelEntityKonfigurasyonu<Gorusme>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Gorusme> builder)
    {
        builder.Property(g => g.Konu).IsRequired().HasMaxLength(200);

        builder.Property(g => g.Notlar).IsRequired().HasMaxLength(4000);

        builder.Property(g => g.AlinanKarar).HasMaxLength(1000);

        builder.HasOne(g => g.Ogrenci)
            .WithMany(o => o.Gorusmeler)
            .HasForeignKey(g => g.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
