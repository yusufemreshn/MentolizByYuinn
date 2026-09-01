using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.DataAccess.Seed;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class KonuConfigurasyonu : TemelEntityKonfigurasyonu<Konu>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Konu> builder)
    {
        builder.Property(k => k.Ad).IsRequired().HasMaxLength(200);

        // ders başka birçok yerde kullanıldığı için silinmesi konu listesini otomatik silmesin
        builder.HasOne(k => k.Ders)
            .WithMany(d => d.Konular)
            .HasForeignKey(k => k.DersId)
            .OnDelete(DeleteBehavior.Restrict);

        // her ders için örnek müfredat konuları kurulumda hazır gelsin diye tohumluyoruz
        builder.HasData(BaslangicVerisi.Konular());
    }
}
