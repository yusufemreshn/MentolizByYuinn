using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mentoliz.Entities;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Context;

public class MentolizDbContext : DbContext
{
    public MentolizDbContext(DbContextOptions<MentolizDbContext> options) : base(options)
    {
    }

    public DbSet<Ogrenci> Ogrenciler => Set<Ogrenci>();
    public DbSet<Veli> Veliler => Set<Veli>();
    public DbSet<Sinif> Siniflar => Set<Sinif>();
    public DbSet<Ders> Dersler => Set<Ders>();
    public DbSet<DersProgramiSatiri> DersProgramiSatirlari => Set<DersProgramiSatiri>();
    public DbSet<OgrenciProgramIstisnasi> OgrenciProgramIstisnalari => Set<OgrenciProgramIstisnasi>();
    public DbSet<Odev> Odevler => Set<Odev>();
    public DbSet<OdevTopluAtama> OdevTopluAtamalari => Set<OdevTopluAtama>();
    public DbSet<SinavTuru> SinavTurleri => Set<SinavTuru>();
    public DbSet<SinavTuruTest> SinavTuruTestleri => Set<SinavTuruTest>();
    public DbSet<Deneme> Denemeler => Set<Deneme>();
    public DbSet<DenemeSonuc> DenemeSonuclari => Set<DenemeSonuc>();
    public DbSet<DenemeSonucDetay> DenemeSonucDetaylari => Set<DenemeSonucDetay>();
    public DbSet<Gorusme> Gorusmeler => Set<Gorusme>();
    public DbSet<Hedef> Hedefler => Set<Hedef>();
    public DbSet<HedefDersNeti> HedefDersNetleri => Set<HedefDersNeti>();
    public DbSet<CalismaProgrami> CalismaProgramlari => Set<CalismaProgrami>();
    public DbSet<CalismaProgramiSatiri> CalismaProgramiSatirlari => Set<CalismaProgramiSatiri>();
    public DbSet<Konu> Konular => Set<Konu>();
    public DbSet<OgrenciKonuTakip> OgrenciKonuTakipleri => Set<OgrenciKonuTakip>();
    public DbSet<Gorev> Gorevler => Set<Gorev>();
    public DbSet<Not> Notlar => Set<Not>();
    public DbSet<Ayar> Ayarlar => Set<Ayar>();
    public DbSet<OgrenciTercihi> Tercihler => Set<OgrenciTercihi>();
    public DbSet<OgrenciDevamsizlik> Devamsizliklar => Set<OgrenciDevamsizlik>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // her entity'nin kendi konfigürasyon sınıfı var, hepsini tek tek burada çağırmak yerine assembly taraması yapıyoruz
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MentolizDbContext).Assembly);

        // yumuşak silinmiş kayıtlar bütün sorgularda otomatik gizlensin diye her entity için aynı filtreyi tek yerden uyguluyoruz
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(SilinmemisFiltresiOlustur(entityType.ClrType));
            }
        }
    }

    // her entity tipi için "SilindiMi == false" ifadesini üreten lambda'yı reflection ile kuruyoruz
    private static LambdaExpression SilinmemisFiltresiOlustur(Type entityTipi)
    {
        var parametre = Expression.Parameter(entityTipi, "kayit");
        var silindiMiOzelligi = Expression.Property(parametre, nameof(BaseEntity.SilindiMi));
        var govde = Expression.Equal(silindiMiOzelligi, Expression.Constant(false));
        return Expression.Lambda(govde, parametre);
    }

    public override int SaveChanges()
    {
        ZamanDamgalariniGuncelle();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ZamanDamgalariniGuncelle();
        return base.SaveChangesAsync(cancellationToken);
    }

    // yeni eklenen kayıtta hem oluşturma hem güncelleme tarihi, güncellenen kayıtta sadece güncelleme tarihi set ediliyor
    private void ZamanDamgalariniGuncelle()
    {
        var simdi = DateTime.Now;

        foreach (var girdi in ChangeTracker.Entries<BaseEntity>())
        {
            if (girdi.State == EntityState.Added)
            {
                girdi.Entity.OlusturmaTarihi = simdi;
                girdi.Entity.GuncellemeTarihi = simdi;
            }
            else if (girdi.State == EntityState.Modified)
            {
                girdi.Entity.GuncellemeTarihi = simdi;
            }
        }
    }
}
