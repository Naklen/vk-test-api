using System.ComponentModel.DataAnnotations.Schema;

namespace vk_test_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }
        public UserGroup Group { get; set; } = null!;

        [ForeignKey("UserState")]
        public int UserStateId { get; set; }
        public UserState State { get; set; } = null!;
    }
}
