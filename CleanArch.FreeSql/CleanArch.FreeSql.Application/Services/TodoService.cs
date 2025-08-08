namespace CleanArch.FreeSql.Application.Services;

using CleanArch.FreeSql.Application.Contracts;
using CleanArch.FreeSql.Application.Interfaces;
using CleanArch.FreeSql.Domain.Abstractions.Repositories;
using CleanArch.FreeSql.Domain.Entities;

public sealed class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<long> CreateAsync(TodoCreateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TodoItem { Title = request.Title };
        return await _todoRepository.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _todoRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<TodoResponse?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _todoRepository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<TodoResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var list = await _todoRepository.ListAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<bool> UpdateAsync(TodoUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TodoItem
        {
            Id = request.Id,
            Title = request.Title,
            IsCompleted = request.IsCompleted
        };
        return await _todoRepository.UpdateAsync(entity, cancellationToken);
    }

    private static TodoResponse Map(TodoItem e) => new(e.Id, e.Title, e.IsCompleted, e.CreatedAtUtc);
}