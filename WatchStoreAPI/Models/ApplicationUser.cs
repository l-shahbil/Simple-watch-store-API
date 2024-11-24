using Microsoft.AspNetCore.Identity;

namespace WatchStoreAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string name { get; set; }
    }
}
