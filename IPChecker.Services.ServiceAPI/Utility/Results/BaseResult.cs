using System.Net;
using System.Net.Http;

namespace IPChecker.Services.ServiceAPI.Utility.Results
{
    public class BaseResult : HttpResponseMessage
    {
        public BaseResult()
        {
            StatusCode = HttpStatusCode.OK;
        }
    }
}
