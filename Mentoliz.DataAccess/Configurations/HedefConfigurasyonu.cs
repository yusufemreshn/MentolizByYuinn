using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class HedefConfigurasyonu : TemelEntityKonfigurasyonu<Hedef>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Hedef> builder)
    {
        builder.Property(h => h.HedefUniversite).IsRequired().HasMaxLength(200);

        builder.Property(h => h.HedefBolum).IsRequired().HasMaxLength(200);

        builder.HasOne(h => h.Ogrenci)
            .WithMany(o => o.Hedefler)
            .HasForeignKey(h => h.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
