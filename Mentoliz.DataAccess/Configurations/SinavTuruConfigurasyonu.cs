using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentoliz.DataAccess.Seed;
using Mentoliz.Entities;

namespace Mentoliz.DataAccess.Configurations;

public class SinavTuruConfigurasyonu : TemelEntityKonfigurasyonu<SinavTuru>
{
    protected override void EntiteyeOzelYapilandir(EntityTypeBuilder<SinavTuru> builder)
    {
        builder.Property(s => s.Ad).IsRequired().HasMaxLength(50);

        builder.Property(s => s.Kod).IsRequired().HasMaxLength(20);

        // aynı kod iki kere tanımlanmasın diye, tyt gibi kodlar tekil olmalı, silinmiş kayıtları saymıyoruz ki kod tekrar kullanılabilsin
        builder.HasIndex(s => s.Kod).IsUnique().HasFilter("SilindiMi = 0");

        // tyt, ayt sayısal, ayt eşit ağırlık, ayt sözel ve ydt kurulumda hazır gelsin diye tohumluyoruz
        builder.HasData(BaslangicVerisi.SinavTurleri());
    }
}
