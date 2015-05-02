using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IPChecker.Services.ServiceAPI.Attributes
{
    //inspired by http://www.strathweb.com/2012/05/output-caching-in-asp-net-web-api/
    public class OutputCacheAttribute : ActionFilterAttribute
    {
        // cache length in seconds
        private readonly TimeSpan _cacheDuration;
        // client cache length in seconds
        private readonly TimeSpan _clientCacheDuration;
        // cache key
        private string _cachekey;
        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        public OutputCacheAttribute(int cacheDurationInSeconds, int clientCacheDurationinSeconds)
        {
            _cacheDuration = TimeSpan.FromSeconds(cacheDurationInSeconds);
            _clientCacheDuration = TimeSpan.FromSeconds(clientCacheDurationinSeconds);
        }

        private CacheControlHeaderValue SetClientCache()
        {
            return new CacheControlHeaderValue {MaxAge = _clientCacheDuration, MustRevalidate = true};
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(WebApiCache.Contains(_cachekey)))
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                WebApiCache.Add(_cachekey, body, DateTime.Now.Add(_cacheDuration));
                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.Add(_cacheDuration));
            }
            actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                var headerKey = "noacceptheader";
                var acceptHeader = actionContext.Request.Headers.Accept.FirstOrDefault();
                if (acceptHeader != null)
                {
                    headerKey = acceptHeader.ToString();
                }
                _cachekey = string.Join(":", new string[] { actionContext.Request.RequestUri.PathAndQuery, headerKey });
                if (WebApiCache.Contains(_cachekey))
                {
                    var val = (string)WebApiCache.Get(_cachekey);
                    if (val != null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse();
                        actionContext.Response.Content = new StringContent(val);
                        var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct");
                        if (contenttype == null)
                        {
                            contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                        }
                        actionContext.Response.Content.Headers.ContentType = contenttype;
                        actionContext.Response.Headers.CacheControl = SetClientCache();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }
    }
}
