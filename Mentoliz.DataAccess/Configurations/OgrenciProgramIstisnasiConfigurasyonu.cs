using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OgrenciProgramIstisnasiConfigurasyonu : TemelEntityKonfigurasyonu<OgrenciProgramIstisnasi>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<OgrenciProgramIstisnasi> builder)
    {
        builder.Property(o => o.OgretmenAdi).HasMaxLength(100);

        builder.Property(o => o.Aciklama).HasMaxLength(1000);

        builder.HasOne(o => o.Ogrenci)
            .WithMany(o => o.ProgramIstisnalari)
            .HasForeignKey(o => o.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);

        // iptal türünde ders boş kalabiliyor, ders silinirse istisna kaydı kalsın ama dersi boşa düşsün
        builder.HasOne(o => o.Ders)
            .WithMany(d => d.ProgramIstisnalari)
            .HasForeignKey(o => o.DersId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
