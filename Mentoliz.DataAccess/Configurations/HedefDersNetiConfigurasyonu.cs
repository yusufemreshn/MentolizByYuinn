using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class HedefDersNetiConfigurasyonu : TemelEntityKonfigurasyonu<HedefDersNeti>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<HedefDersNeti> builder)
    {
        builder.HasOne(h => h.Hedef)
            .WithMany(h => h.DersNetleri)
            .HasForeignKey(h => h.HedefId)
            .OnDelete(DeleteBehavior.Cascade);

        // ders başka yerlerde de kullanıldığı için silinmesi hedef netini otomatik silmesin
        builder.HasOne(h => h.Ders)
            .WithMany(d => d.HedefDersNetleri)
            .HasForeignKey(h => h.DersId)
            .OnDelete(DeleteBehavior.Restrict);

        // aynı hedefte aynı ders iki kere tanımlanmasın
        builder.HasIndex(h => new { h.HedefId, h.DersId }).IsUnique().HasFilter("SilindiMi = 0");
    }
}
