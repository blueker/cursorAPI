using weather.Application.Contracts;
using weather.Application.Services;
using weather.Domain.Abstractions.Repositories;
using weather.Domain.Entities;
using FluentAssertions;
using Moq;

namespace weather.Tests;

public class TodoServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Return_New_Id()
    {
        var repo = new Mock<ITodoRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123);
        var service = new TodoService(repo.Object);

        var id = await service.CreateAsync(new TodoCreateRequest("test"));

        id.Should().Be(123);
        repo.Verify(r => r.AddAsync(It.Is<TodoItem>(x => x.Title == "test"), It.IsAny<CancellationToken>()), Times.Once);
    }
}