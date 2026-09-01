using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class DenemeSonucConfigurasyonu : TemelEntityKonfigurasyonu<DenemeSonuc>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<DenemeSonuc> builder)
    {
        builder.Property(d => d.Notlar).HasMaxLength(1000);

        // deneme silinirse ona ait sonuçların da anlamı kalmıyor
        builder.HasOne(d => d.Deneme)
            .WithMany(d => d.Sonuclar)
            .HasForeignKey(d => d.DenemeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Ogrenci)
            .WithMany(o => o.DenemeSonuclari)
            .HasForeignKey(d => d.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);

        // bir öğrenci aynı denemede birden fazla sonuca sahip olmasın, silinmiş kayıtları saymıyoruz
        builder.HasIndex(d => new { d.DenemeId, d.OgrenciId }).IsUnique().HasFilter("SilindiMi = 0");
    }
}
