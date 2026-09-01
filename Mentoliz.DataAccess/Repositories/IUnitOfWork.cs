using Mentoliz.Entities.Ortak;

namespace Mentoliz.DataAccess.Repositories;

public interface IUnitOfWork
{
    // her entity tipi için tek bir repository örneği veriyor, aynı istek içinde tekrar tekrar yeni repository oluşmasın diye
    IRepository<TEntity> RepositoryGetir<TEntity>() where TEntity : BaseEntity;

    Task<int> KaydetAsync();
}
