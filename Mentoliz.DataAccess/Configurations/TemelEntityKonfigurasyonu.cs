using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Configurations;

// bütün entity konfigürasyonları bundan türeyecek, Id ve BaseEntity alanlarını her seferinde tekrar yazmamak için
public abstract class TemelEntityKonfigurasyonu<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OlusturmaTarihi).IsRequired();

        builder.Property(e => e.GuncellemeTarihi).IsRequired();

        // yeni kayıt eklenirken bu alan elle set edilmezse false olarak düşsün
        builder.Property(e => e.SilindiMi).IsRequired().HasDefaultValue(false);

        EntiteyeOzelYapilandir(builder);
    }

    // her entity kendi özel alanlarını, ilişkilerini ve indekslerini burada tanımlayacak
    protected abstract void EntiteyeOzelYapilandir(EntityTypeBuilder<TEntity> builder);
}
