using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.DataAccess.Seed;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class DersConfigurasyonu : TemelEntityKonfigurasyonu<Ders>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Ders> builder)
    {
        builder.Property(d => d.Ad).IsRequired().HasMaxLength(100);

        builder.Property(d => d.KisaAd).IsRequired().HasMaxLength(20);

        // müfredattaki temel dersler kurulumda hazır gelsin diye tohumluyoruz
        builder.HasData(BaslangicVerisi.Dersler());
    }
}
