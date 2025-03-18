namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class 
    {
        public Task<IReadOnlyList<T>> GetAll();
        public Task<T?> GetByIdAsync(int Id);
        public Task DeleteAsync(T Entity);
        public Task UpdateAsync(T Entity);
        public Task<T> AddAsync(T Entity);
    }
}