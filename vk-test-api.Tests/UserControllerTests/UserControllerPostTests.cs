using Microsoft.AspNetCore.Mvc;
using vk_test_api.Controllers;
using vk_test_api.Utils;

namespace vk_test_api.Tests.UserControllerTests;

public class UserControllerPostTests : TestBase
{
    [Fact]
    public async Task UserControllerPost_AddUser()
    {
        var controller = new UserController(context, cache);
        var commonUserData = new NewUserData
        {
            Login = "user",
            Password = "password",
            IsAdmin = false
        };
        var adminUserData = new NewUserData
        {
            Login = "admin",
            Password = "adminPassword",
            IsAdmin = true
        };

        var resultCommon = (await controller.PostUser(commonUserData)).Result;
        var resultAdmin = (await controller.PostUser(adminUserData)).Result;

        Assert.IsType<NoContentResult>(resultCommon);
        Assert.IsType<NoContentResult>(resultAdmin);
        Assert.NotNull(context.Users.SingleOrDefault(u => u.Login == commonUserData.Login));
        Assert.NotNull(context.Users.SingleOrDefault(u => u.Login == adminUserData.Login));
    }

    [Fact]
    public async Task UserControllerPost_CanNotAddAnotherAdmin()
    {
        var controller = new UserController(context, cache);
        var adminUserData = new NewUserData
        {
            Login = "admin",
            Password = "adminPassword",
            IsAdmin = true
        };
        var newAdmin = new NewUserData
        {
            Login = "newAdmin",
            Password = "pwd",
            IsAdmin = true
        };

        await controller.PostUser(adminUserData);
        var result = (await controller.PostUser(newAdmin)).Result;

        Assert.Equal(1, context.Users.Count(u => u.UserGroupId == (int)UserGroupCodes.Admin));
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UserControllerPost_CanNotAddInLessThenFiveSecond()
    {
        var controller = new UserController(context, cache);
        var sameUser = new NewUserData
        {
            Login = "sameUser",
            Password = "samePass",
            IsAdmin = false
        };

        var firstResult = (await controller.PostUser(sameUser)).Result;
        var secondResult = (await controller.PostUser(sameUser)).Result;

        Thread.Sleep(5500);

        var thirdResult = (await controller.PostUser(sameUser)).Result;

        Assert.IsType<NoContentResult>(firstResult);
        Assert.IsType<NoContentResult>(thirdResult);
        Assert.IsType<BadRequestObjectResult>(secondResult);
        Assert.Equal(2, context.Users.Count(u => u.Login == sameUser.Login));
    }

    [Fact]
    public async Task UserControllerPost_AddAdminWhenOtherBlocked()
    {
        var controller = new UserController(context, cache);
        var adminUserData = new NewUserData
        {
            Login = "admin",
            Password = "adminPassword",
            IsAdmin = true
        };
        var newAdmin = new NewUserData
        {
            Login = "newAdmin",
            Password = "newAdminPass",
            IsAdmin = true
        };

        await controller.PostUser(adminUserData);

        var prevAdmin = context.Users.FirstOrDefault(u => u.UserGroupId == (int)UserGroupCodes.Admin);
        prevAdmin.UserStateId = (int)UserStateCodes.Blocked;
        context.Users.Update(prevAdmin);
        context.SaveChanges();

        var result = (await controller.PostUser(newAdmin)).Result;

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, context.Users.Count(u => u.UserGroupId == (int)UserGroupCodes.Admin && u.UserStateId == (int)UserStateCodes.Active));
    }
}
