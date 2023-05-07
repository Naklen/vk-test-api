using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApiContext _db;
    private readonly IMemoryCache _cache;

    public UserController(ApiContext context, IMemoryCache cache)
    {
        _db = context;
        _cache = cache;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        if (_db.Users == null)
        {
            return NotFound();
        }
        return await _db.Users.Include(u => u.UserGroup).Include(u => u.UserState).ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        if (_db.Users == null)
        {
            return NotFound();
        }
        var user = await _db.Users.Include(u => u.UserGroup).Include(u => u.UserState).FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }    

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(NewUserData newUserData)
    {
        if (_db.Users == null)
            return Problem("Entity set 'ApiContext.Users'  is null.");

        var lastRequestTime = _cache.Get<DateTimeOffset?>(newUserData.Login);
        if (lastRequestTime.HasValue && DateTimeOffset.Now - lastRequestTime < TimeSpan.FromSeconds(5))
            return BadRequest($"To many requests with login {newUserData.Login}");

        _cache.Set(newUserData.Login, DateTimeOffset.Now, TimeSpan.FromSeconds(5));

        if (newUserData.IsAdmin && _db.Users.FirstOrDefault(u => u.UserGroup.Code == UserGroupCodes.Admin.ToString() &&
        u.UserState.Code == UserStateCodes.Active.ToString()) != null)
            return BadRequest("Active admin user already exist");

            User newUser = new()
            {
                Login = newUserData.Login,
                Password = newUserData.Password,
                CreatedDate = DateTime.UtcNow,
                UserGroupId = newUserData.IsAdmin ? (int)UserGroupCodes.Admin : (int)UserGroupCodes.User,
                UserStateId = (int)UserStateCodes.Active
            };
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (_db.Users == null)
        {
            return NotFound();
        }
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (user.UserStateId == (int)UserStateCodes.Blocked)
            return BadRequest($"User {user.Login} already blocked");

        user.UserStateId = (int)UserStateCodes.Blocked;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return (_db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
