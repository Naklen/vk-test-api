using Microsoft.EntityFrameworkCore;
using vk_test_api.Models;

namespace vk_test_api
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;
        public DbSet<UserState> UserStates { get; set; } = null!;        

        public ApiContext (DbContextOptions<ApiContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasData(
                    new UserGroup { Id = (int)Utils.UserGroups.Admin, Code = Utils.UserGroups.Admin.ToString(), Description = "Admin group" },
                    new UserGroup { Id = (int)Utils.UserGroups.User, Code = Utils.UserGroups.User.ToString(), Description = "User group" }
            );
            modelBuilder.Entity<UserState>().HasData(
                    new UserState { Id = (int)Utils.UserStates.Active, Code = Utils.UserStates.Active.ToString(), Description = "Active user" },
                    new UserState { Id = (int)Utils.UserStates.Blocked, Code = Utils.UserStates.Blocked.ToString(), Description = "Blocked user" }
            );
        }
    }
}
