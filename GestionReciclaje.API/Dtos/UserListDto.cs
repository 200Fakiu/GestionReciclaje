using System;

namespace DatingApp.API.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime LastActive { get; set; }
    
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public string PlantName{ get; set; }
    }
}