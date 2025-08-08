namespace weather.Application.Contracts;

public record TodoCreateRequest(string Title);
public record TodoUpdateRequest(long Id, string Title, bool IsCompleted);
public record TodoResponse(long Id, string Title, bool IsCompleted, DateTime CreatedAtUtc);