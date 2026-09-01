using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class GorevConfigurasyonu : TemelEntityKonfigurasyonu<Gorev>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Gorev> builder)
    {
        builder.Property(g => g.Baslik).IsRequired().HasMaxLength(200);

        builder.Property(g => g.Aciklama).HasMaxLength(1000);

        // öğrenciyle ilgisi olmayan genel görevler de var, öğrenci silinirse görev kalsın ama bağlantısı kopsun
        builder.HasOne(g => g.Ogrenci)
            .WithMany(o => o.Gorevler)
            .HasForeignKey(g => g.OgrenciId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
