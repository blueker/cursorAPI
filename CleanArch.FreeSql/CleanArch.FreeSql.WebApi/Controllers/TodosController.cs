using CleanArch.FreeSql.Application.Contracts;
using CleanArch.FreeSql.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.FreeSql.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TodoResponse>>> ListAsync(CancellationToken cancellationToken)
    {
        var list = await _todoService.ListAsync(cancellationToken);
        return Ok(list);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TodoResponse>> GetAsync(long id, CancellationToken cancellationToken)
    {
        var item = await _todoService.GetAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateAsync([FromBody] TodoCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _todoService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAsync), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] TodoUpdateRequest request, CancellationToken cancellationToken)
    {
        var ok = await _todoService.UpdateAsync(request, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var ok = await _todoService.DeleteAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}