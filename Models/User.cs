using System.ComponentModel.DataAnnotations.Schema;

namespace vk_test_api.Models;

[Table("users")]
public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedDate { get; set; }

    [ForeignKey("UserGroup")]
    public int UserGroupId { get; set; }
    public UserGroup UserGroup { get; set; } = null!;

    [ForeignKey("UserState")]
    public int UserStateId { get; set; }
    public UserState UserState { get; set; } = null!;
}
