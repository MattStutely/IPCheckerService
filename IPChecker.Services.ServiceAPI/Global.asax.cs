using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using IPChecker.Services.ServiceAPI.Infrastructure;
using IPChecker.Services.ServiceAPI.Infrastructure.DependencyResolution;
using ServiceAPI.App_Start;
using StructureMap;
using StructureMap.Graph;

namespace IPChecker.Services.ServiceAPI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly ILog _log = LogManager.GetLogger("Global error logger");

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ServiceAccountProxy.SetDefault();
            InitializeStructureMap();
        }

        private static void InitializeStructureMap()
        {
            ObjectFactory.Initialize(
                action => action.Scan(
                    assembly =>
                        {
                            assembly.AssembliesFromApplicationBaseDirectory();
                            assembly.LookForRegistries();
                            assembly.WithDefaultConventions();
                        }));
            GlobalConfiguration.Configuration.DependencyResolver =
                new StructureMapDependencyResolver(ObjectFactory.Container);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            _log.Error("Global error caught", Server.GetLastError());
        }
    }
}
