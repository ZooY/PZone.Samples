using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PZone.Samples.Services;


namespace PZone.Samples.Models
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser, string>
    {
        private readonly AccountService _service;


        public UserStore(AccountService service)
        {
            _service = service;
        }


        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _service.Get(userId);
        }


        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _service.Find(userName);
        }


        public async Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return user.PhoneNumber;
        }


        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return (IList<UserLoginInfo>)new List<UserLoginInfo>
            {
                new UserLoginInfo("Custom", "None")
            };
        }


        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return false;
        }


        public void Dispose()
        {
        }


        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            throw new NotImplementedException();
        }


        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }


        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }


        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }


        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }


        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

    }
}