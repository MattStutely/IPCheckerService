using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using IPChecker.Services.ServiceAPI.Infrastructure;

namespace IPChecker.Services.ServiceAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});
        }
    }
}
