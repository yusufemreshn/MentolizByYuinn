using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mentoliz.DataAccess.Context;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(MentolizDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Sorgu => _dbSet;

    // FindAsync kullanmıyoruz çünkü o global query filter'ı atlıyor, yumuşak silinmiş kayıt da dönebiliyordu
    public async Task<TEntity?> TekGetirAsync(int id) => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<TEntity?> BirIleGetirAsync(Expression<Func<TEntity, bool>> filtre) =>
        await _dbSet.FirstOrDefaultAsync(filtre);

    public async Task<List<TEntity>> ListeleAsync(Expression<Func<TEntity, bool>>? filtre = null) =>
        filtre is null ? await _dbSet.ToListAsync() : await _dbSet.Where(filtre).ToListAsync();

    public async Task<bool> VarMiAsync(Expression<Func<TEntity, bool>> filtre) => await _dbSet.AnyAsync(filtre);

    public async Task EkleAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Guncelle(TEntity entity) => _dbSet.Update(entity);

    public void SilmeyeIsaretle(TEntity entity)
    {
        entity.SilindiMi = true;
        _dbSet.Update(entity);
    }
}
