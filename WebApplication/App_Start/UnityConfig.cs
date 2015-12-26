using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
using System.Web.Configuration;

namespace Folio.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            var indexStorageCS = WebConfigurationManager.ConnectionStrings["IndexStorage"];
            if (indexStorageCS != null)
            {
                container.RegisterInstance<Lucene.Net.Store.Directory>(new AzureDirectory(CloudStorageAccount.Parse(indexStorageCS.ConnectionString), "records", new RAMDirectory()));
            }
            else
                container.RegisterInstance<Lucene.Net.Store.Directory>(FSDirectory.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create), "PureBhakti", "Folio", "records")));

            container.RegisterType<IResourceRepository, Storage.RecordRepository>(new HierarchicalLifetimeManager());
        }
    }
}
