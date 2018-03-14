using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow : IVsWindowSearch
   {
      public IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
      {
         throw new NotImplementedException();
      }

      public void ClearSearch()
      {
      }

      public void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
      {
         //throw new NotImplementedException();
      }

      public bool OnNavigationKeyDown(uint dwNavigationKey, uint dwModifiers)
      {
         return false;
      }

      public bool SearchEnabled => true;
      public Guid Category => Guid.Empty;
      public IVsEnumWindowSearchFilters SearchFiltersEnum => null;
      public IVsEnumWindowSearchOptions SearchOptionsEnum => null;
   }
}
