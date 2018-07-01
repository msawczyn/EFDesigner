namespace Sawczyn.EFDesigner.EFModel {
   public class NuGetDisplay
   {
      public EFVersion EFVersion { get; }
      public string PackageId { get; }
      public string ActualPackageVersion { get; }
      public string DisplayVersion { get; }
      public string MajorMinorVersion { get; }

      public double MajorMinorVersionNum => double.TryParse(MajorMinorVersion, out double result)
                                               ? result
                                               : 0;

      public NuGetDisplay(EFVersion efVersion, string packageId, string packageVersion, string display, string majorMinorVersion)
      {
         EFVersion = efVersion;
         PackageId = packageId;
         ActualPackageVersion = packageVersion;
         DisplayVersion = display;
         MajorMinorVersion = majorMinorVersion;
      }
   }
}