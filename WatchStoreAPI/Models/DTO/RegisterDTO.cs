using System.ComponentModel.DataAnnotations;

namespace WebAPIdemo.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string Email {  get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [Compare("password")]
        public string confirmPassword { get; set; }

    }
}
