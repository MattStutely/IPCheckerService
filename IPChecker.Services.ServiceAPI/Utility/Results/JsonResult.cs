using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IPChecker.Services.ServiceAPI.Utility.Results
{
    public class JsonResult : BaseResult
    {
        public JsonResult(object objectToSerialize)
        {
            Content = new StringContent(SerializeContent(objectToSerialize));
            Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        private string SerializeContent(object objectToSerialize)
        {
            return (JsonConvert.SerializeObject(objectToSerialize));
        }
    }
}
