namespace weather.Infrastructure.Repositories;

using weather.Domain.Abstractions.Repositories;
using weather.Domain.Entities;
using FreeSql;

public sealed class TodoRepository : ITodoRepository
{
    private readonly IFreeSql _fsql;

    public TodoRepository(IFreeSql fsql)
    {
        _fsql = fsql;
        _fsql.CodeFirst.SyncStructure(typeof(TodoItem));
    }

    public async Task<long> AddAsync(TodoItem entity, CancellationToken cancellationToken = default)
    {
        var id = await _fsql.Insert(entity).ExecuteIdentityAsync();
        entity.Id = id;
        return id;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var aff = await _fsql.Delete<TodoItem>(id).ExecuteAffrowsAsync();
        return aff > 0;
    }

    public async Task<TodoItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _fsql.Select<TodoItem>().Where(x => x.Id == id).FirstAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        var items = await _fsql.Select<TodoItem>().OrderByDescending(x => x.Id).ToListAsync();
        return items;
    }

    public async Task<bool> UpdateAsync(TodoItem entity, CancellationToken cancellationToken = default)
    {
        var aff = await _fsql.Update<TodoItem>().SetDto(entity).Where(x => x.Id == entity.Id).ExecuteAffrowsAsync();
        return aff > 0;
    }
}