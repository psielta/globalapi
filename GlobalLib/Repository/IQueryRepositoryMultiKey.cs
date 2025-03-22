namespace GlobalLib.Repository
{
    public interface IQueryRepositoryMultiKey<T, TKey1, TKey2, TDto>
    {
        Task<T?> CreateAsync(TDto dto);
        Task<IQueryable<T>> RetrieveAllAsync();
        Task<T?> RetrieveAsync(TKey1 id1, TKey2 id2);

        Task<T?> RetrieveAsyncAsNoTracking(TKey1 id1, TKey2 id2);
        Task<T?> UpdateAsync(TKey1 id1, TKey2 id2, TDto dto);
        Task<bool?> DeleteAsync(TKey1 id1, TKey2 id2);
    }
}
