using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Identity_EF6NetFramework.Custom
{
   public class ApplicationIdentityRoleStore : IRoleStore<ApplicationRole>
   {
      public Task CreateAsync(ApplicationRole role)
      {
         throw new NotImplementedException();
      }

      public Task DeleteAsync(ApplicationRole role)
      {
         throw new NotImplementedException();
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

      public Task<ApplicationRole> FindByIdAsync(string roleId)
      {
         throw new NotImplementedException();
      }

      public Task<ApplicationRole> FindByNameAsync(string roleName)
      {
         throw new NotImplementedException();
      }

      public Task UpdateAsync(ApplicationRole role)
      {
         throw new NotImplementedException();
      }
   }
}
