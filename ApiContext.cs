using Microsoft.EntityFrameworkCore;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api;

public class ApiContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserGroup> UserGroups { get; set; } = null!;
    public DbSet<UserState> UserStates { get; set; } = null!;

    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGroup>().HasData(
                new UserGroup { Id = (int)UserGroupCodes.Admin, Code = UserGroupCodes.Admin.ToString(), Description = "Admin group" },
                new UserGroup { Id = (int)UserGroupCodes.User, Code = UserGroupCodes.User.ToString(), Description = "User group" }
        );
        modelBuilder.Entity<UserState>().HasData(
                new UserState { Id = (int)UserStateCodes.Active, Code = UserStateCodes.Active.ToString(), Description = "Active user" },
                new UserState { Id = (int)UserStateCodes.Blocked, Code = UserStateCodes.Blocked.ToString(), Description = "Blocked user" }
        );
    }
}
