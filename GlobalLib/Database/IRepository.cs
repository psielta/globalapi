namespace GlobalLib.Database
{
    public interface IRepository<T, TKey>
    {
        Task<T?> CreateAsync(T c);
        Task<IEnumerable<T>> RetrieveAllAsync();
        Task<T?> RetrieveAsync(TKey id);
        Task<T?> UpdateAsync(TKey id, T c);
        Task<bool?> DeleteAsync(TKey id);
    }
}
