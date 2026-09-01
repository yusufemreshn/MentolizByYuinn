using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OdevTopluAtamaConfigurasyonu : TemelEntityKonfigurasyonu<OdevTopluAtama>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<OdevTopluAtama> builder)
    {
        builder.Property(o => o.Baslik).IsRequired().HasMaxLength(200);

        // sınıf silinirse geçmiş toplu atama kaydı sessizce gitmesin, önce elle taşınması gerekiyor
        builder.HasOne(o => o.Sinif)
            .WithMany(s => s.OdevTopluAtamalari)
            .HasForeignKey(o => o.SinifId)
            .OnDelete(DeleteBehavior.Restrict);

        // ders silinirse geçmiş toplu atama kaydı etkilenmesin diye kısıtladık
        builder.HasOne(o => o.Ders)
            .WithMany(d => d.OdevTopluAtamalari)
            .HasForeignKey(o => o.DersId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
