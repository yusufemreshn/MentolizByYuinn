using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class SinifConfigurasyonu : TemelEntityKonfigurasyonu<Sinif>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Sinif> builder)
    {
        builder.Property(s => s.Ad).IsRequired().HasMaxLength(50);
    }
}
