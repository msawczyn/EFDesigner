#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

using Microsoft.Data.ConnectionUI;

namespace Sawczyn.EFDesigner.EFModel
{

   public class DataConnectionConfiguration
   {
      // based on https://github.com/kjbartel/ConnectionDialog/blob/master/Source/Sample/DataConnectionConfiguration.cs   

      private const string configFileName = "DataConnection.xml";
      private readonly string fullFilePath;
      private readonly XDocument xDoc;

      // Available data providers: 
      private IDictionary<string, DataProvider> dataProviders;

      // Available data sources:
      private IDictionary<string, DataSource> dataSources;

      /// <summary>
      ///    Constructor
      /// </summary>
      /// <param name="path">Configuration file path.</param>
      public DataConnectionConfiguration(string path)
      {
         fullFilePath = !string.IsNullOrEmpty(path)
                           ? Path.GetFullPath(Path.Combine(path, configFileName))
                           : Path.Combine(Environment.CurrentDirectory, configFileName);

         if (!string.IsNullOrEmpty(fullFilePath) && File.Exists(fullFilePath))
            xDoc = XDocument.Load(fullFilePath);
         else
         {
            xDoc = new XDocument();
            xDoc.Add(new XElement("ConnectionDialog", new XElement("DataSourceSelection")));
         }

         RootElement = xDoc.Root;
      }

      public XElement RootElement { get; set; }

      public string GetSelectedProvider()
      {
         try
         {
            XElement xElem = RootElement.Element("DataSourceSelection");
            XElement providerElem = xElem.Element("SelectedProvider");

            if (providerElem != null)
               return providerElem.Value;
         }
         catch
         {
            return null;
         }

         return null;
      }

      public string GetSelectedSource()

      {
         try
         {
            XElement xElem = RootElement.Element("DataSourceSelection");
            XElement sourceElem = xElem.Element("SelectedSource");

            if (sourceElem != null)
               return sourceElem.Value;
         }
         catch
         {
            return null;
         }

         return null;
      }

      public void LoadConfiguration(DataConnectionDialog dialog)

      {
         dialog.DataSources.Add(DataSource.SqlDataSource);
         dialog.DataSources.Add(DataSource.SqlFileDataSource);
         dialog.DataSources.Add(DataSource.OracleDataSource);
         dialog.DataSources.Add(DataSource.AccessDataSource);
         dialog.DataSources.Add(DataSource.OdbcDataSource);

         dialog.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);
         dialog.UnspecifiedDataSource.Providers.Add(DataProvider.OracleDataProvider);
         dialog.UnspecifiedDataSource.Providers.Add(DataProvider.OleDBDataProvider);
         dialog.UnspecifiedDataSource.Providers.Add(DataProvider.OdbcDataProvider);
         dialog.DataSources.Add(dialog.UnspecifiedDataSource);

         dataSources = new Dictionary<string, DataSource>
                       {
                          { DataSource.SqlDataSource.Name, DataSource.SqlDataSource }
                        , { DataSource.SqlFileDataSource.Name, DataSource.SqlFileDataSource }
                        , { DataSource.OracleDataSource.Name, DataSource.OracleDataSource }
                        , { DataSource.AccessDataSource.Name, DataSource.AccessDataSource }
                        , { DataSource.OdbcDataSource.Name, DataSource.OdbcDataSource }
                        , { dialog.UnspecifiedDataSource.DisplayName, dialog.UnspecifiedDataSource }
                       };

         dataProviders = new Dictionary<string, DataProvider>
                         {
                            { DataProvider.SqlDataProvider.Name, DataProvider.SqlDataProvider }
                          , { DataProvider.OracleDataProvider.Name, DataProvider.OracleDataProvider }
                          , { DataProvider.OleDBDataProvider.Name, DataProvider.OleDBDataProvider }
                          , { DataProvider.OdbcDataProvider.Name, DataProvider.OdbcDataProvider }
                         };

         string dsName = GetSelectedSource();

         if (!string.IsNullOrEmpty(dsName) && dataSources.TryGetValue(dsName, out DataSource ds))
            dialog.SelectedDataSource = ds;

         string dpName = GetSelectedProvider();

         if (!string.IsNullOrEmpty(dpName) && dataProviders.TryGetValue(dpName, out DataProvider dp))
            dialog.SelectedDataProvider = dp;
      }

      // ReSharper disable once UnusedMember.Global
      public void SaveConfiguration(DataConnectionDialog dcd)

      {
         if (dcd.SaveSelection)
         {
            DataSource ds = dcd.SelectedDataSource;

            if (ds != null)
            {
               SaveSelectedSource(ds == dcd.UnspecifiedDataSource
                                     ? ds.DisplayName
                                     : ds.Name);
            }

            DataProvider dp = dcd.SelectedDataProvider;

            if (dp != null)
               SaveSelectedProvider(dp.Name);

            xDoc.Save(fullFilePath);
         }
      }

      public void SaveSelectedProvider(string provider)

      {
         if (!string.IsNullOrEmpty(provider))
         {
            try
            {
               XElement xElem = RootElement.Element("DataSourceSelection");
               XElement sourceElem = xElem.Element("SelectedProvider");

               if (sourceElem != null)
                  sourceElem.Value = provider;
               else
                  xElem.Add(new XElement("SelectedProvider", provider));
            }

            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
         }
      }

      public void SaveSelectedSource(string source)

      {
         if (!string.IsNullOrEmpty(source))
         {
            try
            {
               XElement xElem = RootElement.Element("DataSourceSelection");
               XElement sourceElem = xElem.Element("SelectedSource");

               if (sourceElem != null)
                  sourceElem.Value = source;
               else
                  xElem.Add(new XElement("SelectedSource", source));
            }

            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
         }
      }
   }

}