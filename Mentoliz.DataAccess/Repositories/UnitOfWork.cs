using Mentoliz.DataAccess.Context;
using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MentolizDbContext _context;

    // aynı entity tipi için repository ikinci kez istenirse yenisini kurmadan öbekten veriyoruz
    private readonly Dictionary<Type, object> _repositoryOnbellegi = new();

    public UnitOfWork(MentolizDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> RepositoryGetir<TEntity>() where TEntity : BaseEntity
    {
        if (_repositoryOnbellegi.TryGetValue(typeof(TEntity), out var mevcutRepository))
        {
            return (IRepository<TEntity>)mevcutRepository;
        }

        var yeniRepository = new Repository<TEntity>(_context);
        _repositoryOnbellegi[typeof(TEntity)] = yeniRepository;
        return yeniRepository;
    }

    public Task<int> KaydetAsync() => _context.SaveChangesAsync();
}
