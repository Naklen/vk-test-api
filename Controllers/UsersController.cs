using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApiContext _db;

    public UsersController(ApiContext context)
    {
        _db = context;
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

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _db.Entry(user).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(NewUserData newUserData)
    {
        if (_db.Users == null)
        {
            return Problem("Entity set 'ApiContext.Users'  is null.");
        }
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

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return (_db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
