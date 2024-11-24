using System.ComponentModel.DataAnnotations;

namespace WatchStoreAPI.Models.DTO.AccountDto
{
    public class loginDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }

    }
}
