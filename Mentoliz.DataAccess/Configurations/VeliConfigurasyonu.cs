using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class VeliConfigurasyonu : TemelEntityKonfigurasyonu<Veli>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Veli> builder)
    {
        builder.Property(v => v.Ad).IsRequired().HasMaxLength(100);

        builder.Property(v => v.Soyad).IsRequired().HasMaxLength(100);

        builder.Property(v => v.Telefon).IsRequired().HasMaxLength(20);

        builder.Property(v => v.Meslek).HasMaxLength(100);

        builder.Property(v => v.Eposta).HasMaxLength(200);

        // veli, öğrencisinden bağımsız var olamaz, öğrenci silinirse veli kaydı da gider
        builder.HasOne(v => v.Ogrenci)
            .WithMany(o => o.Veliler)
            .HasForeignKey(v => v.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
