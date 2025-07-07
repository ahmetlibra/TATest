namespace TKApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
