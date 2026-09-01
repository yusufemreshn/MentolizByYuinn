using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OgrenciTercihiConfigurasyonu : TemelEntityKonfigurasyonu<OgrenciTercihi>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<OgrenciTercihi> builder)
    {
        builder.Property(t => t.UniversiteAdi).IsRequired().HasMaxLength(200);

        builder.Property(t => t.BolumAdi).IsRequired().HasMaxLength(200);

        builder.Property(t => t.Notlar).HasMaxLength(500);

        builder.HasOne(t => t.Ogrenci)
            .WithMany(o => o.Tercihler)
            .HasForeignKey(t => t.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
