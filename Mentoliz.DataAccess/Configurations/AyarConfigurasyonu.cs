using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class AyarConfigurasyonu : TemelEntityKonfigurasyonu<Ayar>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Ayar> builder)
    {
        builder.Property(a => a.Anahtar).IsRequired().HasMaxLength(100);

        builder.Property(a => a.Deger).IsRequired().HasMaxLength(1000);

        // aynı ayar anahtarı iki kere tanımlanmasın
        builder.HasIndex(a => a.Anahtar).IsUnique().HasFilter("SilindiMi = 0");
    }
}
