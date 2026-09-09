using System.ComponentModel.DataAnnotations;

namespace MovieApplicationUI.Dto.Register
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "It must be the same as the password")]
        public string ComparePassword { get; set; }
    }
}
