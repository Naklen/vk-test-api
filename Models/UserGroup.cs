using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vk_test_api.Models;

[Table("user_groups")]
public class UserGroup
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}
