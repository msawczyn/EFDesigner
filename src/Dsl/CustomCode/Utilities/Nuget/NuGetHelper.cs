using Sawczyn.EFDesigner.EFModel.Nuget;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Sawczyn.EFDesigner.EFModel
{
    public class NuGetHelper
    {
        public const string PACKAGEID_EF6 = "EntityFramework";
        public const string PACKAGEID_EFCORE = "Microsoft.EntityFrameworkCore";

        private const string NUGET_URL = "https://api-v2v3search-0.nuget.org/query?q={0}&prerelease=true&semVerLevel=2.0.0";
        private static readonly HttpClient httpClient = new HttpClient();

        static NuGetHelper()
        {
            EFPackageVersions = new Dictionary<EFVersion, IEnumerable<string>>();
            NuGetPackageDisplay = new List<NuGetDisplay>();

            try
            {
                LoadNuGetVersions(EFVersion.EF6, PACKAGEID_EF6);
                LoadNuGetVersions(EFVersion.EFCore, PACKAGEID_EFCORE);
            }
            catch
            {
                EFPackageVersions.Clear();
                EFPackageVersions.Add(EFVersion.EF6, new List<string>());
                EFPackageVersions.Add(EFVersion.EFCore, new List<string>());

                NuGetPackageDisplay.Clear();
            }

        }

        public static Dictionary<EFVersion, IEnumerable<string>> EFPackageVersions { get; }
        public static List<NuGetDisplay> NuGetPackageDisplay { get; }

        private static void LoadNuGetVersions(EFVersion efVersion, string packageId)
        {
            // get NuGet packages with that package id
            HttpResponseMessage responseMessage = httpClient.GetAsync(string.Format(NUGET_URL, packageId)).GetAwaiter().GetResult();
            string jsonString = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            NuGetPackages nugetPackages = NuGetPackages.FromJson(jsonString);
            string id = packageId.ToLower();

            // get their versions
            List<string> result = nugetPackages.Data
                                               .Where(x => x.Title.ToLower() == id)
                                               .SelectMany(x => x.Versions)
                                               .OrderBy(v => v.VersionVersion)
                                               .Select(v => v.VersionVersion)
                                               .ToList();

            // find the major.minor versions
            List<string> majorVersions = result.Select(v => string.Join(".", v.Split('.').Take(2))).OrderBy(v => v).Distinct().ToList();

            // do the trivial mapping of the full version to the full display name
            foreach (string v in result)
                NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, packageId, v, v, string.Join(".", v.Split('.').Take(2))));

            // figure out which one is the latest in the major.minor set and add its mapping
            foreach (string v in majorVersions)
                NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, packageId, result.FindLast(x => x.StartsWith($"{v}.")), $"{v}.Latest", v));

            // figure out which is the overall latest and map it
            NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, packageId, result.FindLast(x => !x.EndsWith(".Latest")), "Latest", majorVersions.Last()));

            // tuck it away
            EFPackageVersions.Add(efVersion, NuGetPackageDisplay.Where(p => p.EFVersion == efVersion).Select(p => p.DisplayVersion).ToList());
        }
    }
}
