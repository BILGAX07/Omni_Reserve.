
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.IntegrationTests;

public class RoomsControllerTests
{
    [Fact]
    public async Task GetAllRooms_ReturnsOk()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/rooms");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomDoesNotExist_ReturnsNotFound()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(
            $"/api/rooms/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateRoom_WithInvalidJson_ReturnsBadRequest()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var content = new StringContent(
            "{",
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            "/api/rooms",
            content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDistributedCache>();
            services.AddDistributedMemoryCache();

            services.RemoveAll<IRoomRepository>();
            services.AddSingleton<IRoomRepository, FakeRoomRepository>();
        });
    }
}

public class FakeRoomRepository : IRoomRepository
{
    public Task AddAsync(Room room)
    {
        return Task.CompletedTask;
    }

    public Task<Room?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<Room?>(null);
    }

    public Task<Room?> SearchByNumberAsync(string roomNumber)
    {
        return Task.FromResult<Room?>(null);
    }

    public Task<List<Room>> GetRoomsByTypeRawAsync(string roomType)
    {
        return Task.FromResult(new List<Room>());
    }

    public Task<List<Room>> GetAllAsync()
    {
        return Task.FromResult(new List<Room>());
    }
}
