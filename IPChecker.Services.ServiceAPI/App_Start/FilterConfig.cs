using System.Web.Mvc;
using IPChecker.Services.ServiceAPI.Infrastructure;

namespace IPChecker.Services.ServiceAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
