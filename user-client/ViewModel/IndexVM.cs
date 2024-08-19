using user_client.Dto;

namespace user_client.ViewModel
{
    public class IndexVM
    {
        public string? Token { get; set; }

        public List<UserDto> Users { get; set; } = [];
    }
}
