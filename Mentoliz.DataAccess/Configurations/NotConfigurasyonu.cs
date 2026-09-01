using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class NotConfigurasyonu : TemelEntityKonfigurasyonu<Not>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Not> builder)
    {
        builder.Property(n => n.Baslik).IsRequired().HasMaxLength(200);

        builder.Property(n => n.Icerik).IsRequired().HasMaxLength(4000);
    }
}
