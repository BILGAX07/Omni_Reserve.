using System.Net;
using System.Text;

namespace OmniReserve.IntegrationTests;

public class RoomsControllerTests
{
    [Fact]
    public async Task GetAllRooms_ReturnsOk()
    {
        using var factory = new OmniReserveWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/rooms");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomDoesNotExist_ReturnsNotFound()
    {
        using var factory = new OmniReserveWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(
            $"/api/rooms/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateRoom_WithInvalidJson_ReturnsBadRequest()
    {
        using var factory = new OmniReserveWebApplicationFactory();
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

    [Fact]
    public async Task CreateRoom_ReturnsCreatedResponse()
    {
        using var factory = new OmniReserveWebApplicationFactory();
        using var client = factory.CreateClient();

        var json = """
        {
            "roomNumber": "999",
            "type": 2,
            "pricePerNight": 500
        }
        """;

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            "/api/rooms",
            content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
