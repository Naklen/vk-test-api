using Microsoft.EntityFrameworkCore;

namespace vk_test_api.Tests;

public class ApiContextFactory
{
    public static ApiContext Create()
    {
        var options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new ApiContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static void Destroy(ApiContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
