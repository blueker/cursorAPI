namespace weather.Application.Interfaces;

using weather.Application.Contracts;

public interface ITodoService
{
    Task<TodoResponse?> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoResponse>> ListAsync(CancellationToken cancellationToken = default);
    Task<long> CreateAsync(TodoCreateRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TodoUpdateRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}