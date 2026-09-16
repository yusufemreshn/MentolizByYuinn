using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class OgrenciDevamsizlikKonfigurasyonu : TemelEntityKonfigurasyonu<OgrenciDevamsizlik>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<OgrenciDevamsizlik> builder)
    {
        builder.Property(d => d.Not).HasMaxLength(300);

        builder.HasOne(d => d.Ogrenci)
            .WithMany(o => o.Devamsizliklar)
            .HasForeignKey(d => d.OgrenciId)
            .OnDelete(DeleteBehavior.Cascade);

        // bir öğrenci ve bir gün için tekrar sorgusu sık çalışacağı için indeks koyduk, benzersizlik zorunluluğunu veritabanı seviyesinde değil servis içinde kontrol ediyoruz çünkü yumuşak silinen bir kayıt aynı güne yeniden eklenebilmeli
        builder.HasIndex(d => new { d.OgrenciId, d.Tarih });
    }
}
