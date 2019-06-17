using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Identity_EF6NetFramework.Custom
{
   class ApplicationIdentityUserStore : IUserStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
   {
      public Task AddToRoleAsync(ApplicationUser user, string roleName)
      {
         throw new NotImplementedException();
      }

      public Task CreateAsync(ApplicationUser user)
      {
         throw new NotImplementedException();
      }

      public Task DeleteAsync(ApplicationUser user)
      {
         throw new NotImplementedException();
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

      public Task<ApplicationUser> FindByIdAsync(string userId)
      {
         throw new NotImplementedException();
      }

      public Task<ApplicationUser> FindByNameAsync(string userName)
      {
         throw new NotImplementedException();
      }

      public Task<IList<string>> GetRolesAsync(ApplicationUser user)
      {
         throw new NotImplementedException();
      }

      public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
      {
         throw new NotImplementedException();
      }

      public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
      {
         throw new NotImplementedException();
      }

      public Task UpdateAsync(ApplicationUser user)
      {
         throw new NotImplementedException();
      }
   }
}
