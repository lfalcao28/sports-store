namespace WebUI
{
    using System.Configuration;
    using System.Data.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MVCGenericLibrary.Controller;
    using SportsStore.DomainModel.Repositories;
    using StructureMap;
    using StructureMap.Attributes;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PagedIndexData", // Route name
                "{controller}/Page/{page}", // URL with parameters
                new { action = "Index", page = "1" } // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{key}",                           // URL with parameters
                new { controller = "Products", action = "Index", key = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory(new MVCGenericLibraryControllerFactory());
            InitStructureMap();
            RegisterRoutes(RouteTable.Routes);
        }

        private void InitStructureMap()
        {
            ObjectFactory.Initialize(
                x =>
                    {
                        x.ForRequestedType<IProductRepository>().
                            TheDefaultIsConcreteType<ProductLinqToSqlRepository>().CacheBy(InstanceScope.HttpContext);
                        x.ForRequestedType<ICategoryRepository>().
                            TheDefaultIsConcreteType<CategoryLinqToSqlRepository>().CacheBy(InstanceScope.HttpContext);

                        // FakeMemoryRepository with no data
                        //x.ForRequestedType<IProductRepository>().CacheBy(InstanceScope.HttpContext).
                        //    TheDefault.Is.ConstructedBy(() => new FakeProductRepository(null));

                        x.ForRequestedType<DataContext>().CacheBy(InstanceScope.HttpContext).TheDefault.Is.ConstructedBy(() => new DataContext(ConfigurationManager.ConnectionStrings["SportsStore"].ConnectionString));
                    });
        }
    }
}