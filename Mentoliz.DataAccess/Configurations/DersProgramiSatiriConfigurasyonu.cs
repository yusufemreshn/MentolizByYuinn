using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class DersProgramiSatiriConfigurasyonu : TemelEntityKonfigurasyonu<DersProgramiSatiri>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<DersProgramiSatiri> builder)
    {
        builder.Property(d => d.DerslikAdi).HasMaxLength(50);

        builder.Property(d => d.OgretmenAdi).HasMaxLength(100);

        // program satırı sınıfsız var olamaz, sınıf silinince satırlar da gider
        builder.HasOne(d => d.Sinif)
            .WithMany(s => s.DersProgramiSatirlari)
            .HasForeignKey(d => d.SinifId)
            .OnDelete(DeleteBehavior.Cascade);

        // ders başka birçok yerde kullanıldığı için dersin silinmesi program satırını otomatik silmesin, önce elle taşınması gerekiyor
        builder.HasOne(d => d.Ders)
            .WithMany(d => d.DersProgramiSatirlari)
            .HasForeignKey(d => d.DersId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
