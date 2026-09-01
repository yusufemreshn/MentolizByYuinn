using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class DenemeSonucDetayConfigurasyonu : TemelEntityKonfigurasyonu<DenemeSonucDetay>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<DenemeSonucDetay> builder)
    {
        builder.HasOne(d => d.DenemeSonuc)
            .WithMany(d => d.Detaylar)
            .HasForeignKey(d => d.DenemeSonucId)
            .OnDelete(DeleteBehavior.Cascade);

        // test tanımı başka denemelerde de kullanıldığı için silinmesi geçmiş detay kayıtlarını silmesin
        builder.HasOne(d => d.SinavTuruTest)
            .WithMany(s => s.SonucDetaylari)
            .HasForeignKey(d => d.SinavTuruTestId)
            .OnDelete(DeleteBehavior.Restrict);

        // aynı sonuç içinde aynı test iki kere girilmesin
        builder.HasIndex(d => new { d.DenemeSonucId, d.SinavTuruTestId }).IsUnique().HasFilter("SilindiMi = 0");
    }
}
