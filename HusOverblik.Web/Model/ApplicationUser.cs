using Microsoft.AspNetCore.Identity;

namespace HusOverblik.Web.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
