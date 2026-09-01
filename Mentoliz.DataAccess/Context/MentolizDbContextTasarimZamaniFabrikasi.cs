using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mentoliz.DataAccess.Context;

// dotnet ef komutları çalıştırılırken sadece migration üretmek için kullanılıyor, uygulamanın kendisi gerçek bağlantı dizesini Web katmanından ayrı bir yerden alacak
public class MentolizDbContextTasarimZamaniFabrikasi : IDesignTimeDbContextFactory<MentolizDbContext>
{
    public MentolizDbContext CreateDbContext(string[] args)
    {
        var secenekler = new DbContextOptionsBuilder<MentolizDbContext>()
            .UseSqlite("Data Source=tasarim_zamani.db")
            .Options;

        return new MentolizDbContext(secenekler);
    }
}
