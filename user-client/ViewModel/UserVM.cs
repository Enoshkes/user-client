﻿namespace user_client.ViewModel
{
    public class UserVM
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public  string Email { get; set; }
        public  string Password { get; set; }
        public IFormFile? Image { get; set; }
        public List<UserVM> Friends { get; set; } = [];
    }
}
