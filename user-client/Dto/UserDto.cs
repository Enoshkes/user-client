using user_client.ViewModel;

namespace user_client.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[]? Image { get; set; }
        public List<UserVM> Friends { get; set; } = [];
    }
}
