﻿namespace GlobalLib.Repository
{
    public interface IQueryRepository<T, TKey, TDto>
    {
        Task<T?> CreateAsync(TDto dto);
        Task<IQueryable<T>> RetrieveAllAsync();
        Task<T?> RetrieveAsync(TKey id);
        Task<T?> RetrieveAsyncAsNoTracking(TKey id);
        Task<T?> UpdateAsync(TKey id, TDto dto);
        Task<bool?> DeleteAsync(TKey id);
        Task<IEnumerable<T>?> CreateBulkAsync(IEnumerable<TDto> dtos);

    }
}
