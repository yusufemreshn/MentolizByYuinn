using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class DenemeConfigurasyonu : TemelEntityKonfigurasyonu<Deneme>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<Deneme> builder)
    {
        builder.Property(d => d.Ad).IsRequired().HasMaxLength(200);

        builder.Property(d => d.YayinAdi).HasMaxLength(100);

        builder.Property(d => d.Aciklama).HasMaxLength(1000);

        // geçmiş deneme kayıtları önemli veri, sınav türü silinirken kazara gitmesin diye kısıtladık
        builder.HasOne(d => d.SinavTuru)
            .WithMany(s => s.Denemeler)
            .HasForeignKey(d => d.SinavTuruId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
