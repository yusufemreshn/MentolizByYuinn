using System.Linq.Expressions;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Repositories;

// bütün entity'lerde ortak olan veri erişim işlemleri burada, özel sorgu ihtiyacı olan modüllerde ayrıca kendi repository'si yazılacak
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> TekGetirAsync(int id);

    Task<TEntity?> BirIleGetirAsync(Expression<Func<TEntity, bool>> filtre);

    Task<List<TEntity>> ListeleAsync(Expression<Func<TEntity, bool>>? filtre = null);

    Task<bool> VarMiAsync(Expression<Func<TEntity, bool>> filtre);

    // include gerektiren karmaşık sorgular için doğrudan sorgulanabilir erişim
    IQueryable<TEntity> Sorgu { get; }

    Task EkleAsync(TEntity entity);

    void Guncelle(TEntity entity);

    // kayıt gerçekten silinmiyor, sadece SilindiMi alanı işaretleniyor
    void SilmeyeIsaretle(TEntity entity);
}
