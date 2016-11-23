using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;


namespace PZone.Samples.Models
{
    public class ApplicationUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}