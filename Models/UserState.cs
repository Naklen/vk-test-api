using Microsoft.Build.Framework;

namespace vk_test_api.Models;

public class UserState
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}
