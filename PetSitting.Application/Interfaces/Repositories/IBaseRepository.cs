using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class 
    {
        public Task SaveChangesAsync();
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int Id);
        public Task<T?> FirstOrDefaultAsync(Expression<Func<T,bool>> predicate); 
        public void DeleteAsync(T Entity);
        public void UpdateAsync(T Entity);
        public Task<T> AddAsync(T Entity);
    }
}