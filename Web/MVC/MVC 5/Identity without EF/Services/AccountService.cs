using System;
using System.Linq;
using System.Threading.Tasks;
using PZone.Samples.Models;


namespace PZone.Samples.Services
{
public class AccountService : IDisposable
{
    private readonly ApplicationUser[] _sampleUsers =
    {
        new ApplicationUser { Id = "alpha@mail.ru", UserName = "alpha@mail.ru", PasswordHash = "123456" },
        new ApplicationUser { Id = "beta@mail.ru", UserName = "beta@mail.ru", PasswordHash = "6543211" },
        new ApplicationUser { Id = "demo@mail.ru", UserName = "demo@mail.ru", PasswordHash = "456123" }
    };


    public static AccountService Create()
    {
        return new AccountService();
    }
        
    public async Task<ApplicationUser> Get(string userId)
    {
        var id = userId.ToLower();
        return _sampleUsers.First(u => u.Id.Equals(id));
    }


    public async Task<ApplicationUser> Find(string userName)
    {
        var name = userName.ToLower();
        return _sampleUsers.FirstOrDefault(u => u.UserName.Equals(name));
    }


    public void Dispose()
    {
    }
}
}