namespace CleanArch.FreeSql.Domain.Abstractions.Repositories;

using CleanArch.FreeSql.Domain.Entities;

public interface ITodoRepository
{
    Task<TodoItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken = default);
    Task<long> AddAsync(TodoItem entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TodoItem entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}