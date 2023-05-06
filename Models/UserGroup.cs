using System.ComponentModel.DataAnnotations;

namespace vk_test_api.Models;

public class UserGroup
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}
