using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.DataAccess.Seed;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class SinavTuruTestConfigurasyonu : TemelEntityKonfigurasyonu<SinavTuruTest>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<SinavTuruTest> builder)
    {
        // test tanımı sınav türüne ait, tür silinirse test tanımlarının da gitmesi mantıklı
        builder.HasOne(s => s.SinavTuru)
            .WithMany(s => s.Testler)
            .HasForeignKey(s => s.SinavTuruId)
            .OnDelete(DeleteBehavior.Cascade);

        // ders başka yerlerde de kullanıldığı için silinmesi test tanımını otomatik silmesin
        builder.HasOne(s => s.Ders)
            .WithMany(d => d.SinavTuruTestleri)
            .HasForeignKey(s => s.DersId)
            .OnDelete(DeleteBehavior.Restrict);

        // her sınav türünün hangi dersten kaç soru sorduğu kurulumda hazır gelsin diye tohumluyoruz
        builder.HasData(BaslangicVerisi.SinavTuruTestleri());
    }
}
