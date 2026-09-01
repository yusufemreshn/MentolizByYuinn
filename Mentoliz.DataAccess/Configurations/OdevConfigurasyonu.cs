using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OdevConfigurasyonu : TemelEntityKonfigurasyonu<Odev>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Odev> builder)
    {
        builder.Property(o => o.KonuBasligi).IsRequired().HasMaxLength(200);

        builder.Property(o => o.KaynakAdi).HasMaxLength(200);

        builder.Property(o => o.Aciklama).HasMaxLength(1000);

        builder.HasOne(o => o.Ogrenci)
            .WithMany(o => o.Odevler)
            .HasForeignKey(o => o.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);

        // ders silinirse geçmiş ödev kayıtları etkilenmesin diye kısıtladık
        builder.HasOne(o => o.Ders)
            .WithMany(d => d.Odevler)
            .HasForeignKey(o => o.DersId)
            .OnDelete(DeleteBehavior.Restrict);

        // toplu atama silinirse bireysel ödev kaydı kalsın, sadece toplu atama bağlantısı kopsun
        builder.HasOne(o => o.TopluAtama)
            .WithMany(t => t.Odevler)
            .HasForeignKey(o => o.TopluAtamaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
