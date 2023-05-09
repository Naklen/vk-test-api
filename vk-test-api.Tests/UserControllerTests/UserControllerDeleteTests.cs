using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vk_test_api.Controllers;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api.Tests.UserControllerTests;

public class UserControllerDeleteTests : TestBase
{
    [Fact]
    public async Task UserControllerDelete_NotFoundUser()
    {
        var controller = new UserController(context, cache);

        var result = await controller.DeleteUser(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UserControllerDelete_UserAlreadyBlocked()
    {
        var user = new User()
        {
            Id = 1,
            Login = "login",
            Password = "password",
            UserGroupId = 2,
            UserStateId = 2,
            CreatedDate = DateTime.Now
        };
        context.Users.Add(user);
        context.SaveChanges();
        var controller = new UserController(context, cache);

        var result = await controller.DeleteUser(1);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UserControllerDelet_DeleteUser()
    {
        var user = new User()
        {
            Id = 1,
            Login = "login",
            Password = "password",
            UserGroupId = 2,
            UserStateId = 1,
            CreatedDate = DateTime.Now
        };
        context.Users.Add(user);
        context.SaveChanges();
        var controller = new UserController(context, cache);

        var result = await controller.DeleteUser(user.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(2, context.Users.First().UserStateId);
    }
}
