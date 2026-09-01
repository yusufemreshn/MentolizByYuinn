using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OgrenciKonuTakipConfigurasyonu : TemelEntityKonfigurasyonu<OgrenciKonuTakip>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<OgrenciKonuTakip> builder)
    {
        builder.Property(o => o.Notlar).HasMaxLength(1000);

        builder.HasOne(o => o.Ogrenci)
            .WithMany(o => o.KonuTakipleri)
            .HasForeignKey(o => o.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);

        // konu silinirse öğrencinin o konudaki geçmiş takip kaydı kalsın diye kısıtladık
        builder.HasOne(o => o.Konu)
            .WithMany(k => k.OgrenciTakipleri)
            .HasForeignKey(o => o.KonuId)
            .OnDelete(DeleteBehavior.Restrict);

        // bir öğrencinin bir konu için tek bir takip kaydı olsun
        builder.HasIndex(o => new { o.OgrenciId, o.KonuId }).IsUnique().HasFilter("SilindiMi = 0");
    }
}
