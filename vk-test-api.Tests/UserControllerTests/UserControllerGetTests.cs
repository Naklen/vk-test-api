using Microsoft.AspNetCore.Mvc;
using vk_test_api.Controllers;

namespace vk_test_api.Tests.UserControllerTests;

public class UserControllerGetTests : TestBase
{
    [Fact]
    public async Task UserControllerGet_NoUsers()
    {
        var controller = new UserController(context, cache);

        var result = (await controller.GetUsers()).Value;

        Assert.Empty(result);
    }

    [Fact]
    public async Task UserControllerGet_AllUsers()
    {
        FillDataBase(context);
        var controller = new UserController(context, cache);

        var result = (await controller.GetUsers()).Value;

        Assert.Equal(15, result.Count());
    }

    [Fact]
    public async Task UserControllerGet_NotFoundUser()
    {
        var controller = new UserController(context, cache);

        var result = (await controller.GetUser(1)).Result;

        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(1, "John Smith", "CRxXNUblDKoSnYfbdse")]
    [InlineData(15, "John Lefebvre", "vAxQZTbuCLqL")]
    [InlineData(7, "Jane Doe", "hIRpVexzBkUBujqSyCMM")]
    public async Task UserControllerGet_GetUser(int id, string login, string password)
    {
        FillDataBase(context);
        var controller = new UserController(context, cache);

        var result = (await controller.GetUser(id)).Value;

        Assert.NotNull(result);
        Assert.Equal(login, result.Login);
        Assert.Equal(password, result.Password);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(-1, 1)]
    [InlineData(-1, -1)]
    public async Task UserControllerGet_PaginationBadRequest(int limit, int offset)
    {
        var controller = new UserController(context, cache);

        var result = (await controller.GetPaginatedUsers(limit, offset)).Result;

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(15, 0)]
    [InlineData(4, 10)]
    [InlineData(5, 12)]
    public async Task UserControllerGet_Pagination(int limit, int offset)
    {
        FillDataBase(context);
        var controller = new UserController(context, cache);

        var result = (await controller.GetPaginatedUsers(limit, offset)).Value;

        Assert.Equal(Math.Min(15 - offset, limit), result.Count());
    }
}
