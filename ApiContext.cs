using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api;

public class ApiContext : DbContext
{
    const bool NeedToSample = true;
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
        if ((bool)JObject.Parse(File.ReadAllText(@".\appsettings.json")).GetValue("SampleData"))
        {
            var JsonSample = JArray.Parse(File.ReadAllText(@".\dataSample.json"));
            var sample = new List<User>();
            var i = 1;
            foreach (JObject item in JsonSample)
            {
                sample.Add(new User
                {
                    Id = i,
                    Login = item.GetValue("login").ToString(),
                    Password = item.GetValue("password").ToString(),
                    CreatedDate = DateTime.UtcNow + TimeSpan.FromDays(new Random().Next(-2000, 2000)),
                    UserGroupId = (int)UserGroupCodes.User,
                    UserStateId = (int)UserStateCodes.Active
                });
                i++;
            }
            modelBuilder.Entity<User>().HasData(sample);
        }
    }
}
