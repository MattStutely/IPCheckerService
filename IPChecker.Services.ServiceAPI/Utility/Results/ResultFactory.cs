using System;
using System.Net.Http;

namespace IPChecker.Services.ServiceAPI.Utility.Results
{
    public static class ResultFactory
    {
        /// <summary>
        /// Creates an appropriate response to return from the controller
        /// </summary>
        /// <param name="objectToSerialize">The items you want to return</param>
        /// <param name="format">The format the user has requested</param>
        /// <returns>A formatted response</returns>
        public static HttpResponseMessage CreateResult(object objectToSerialize, string format)
        {
            BaseResult result;
            switch (format)
            {
                case "xml":
                    result = new XmlResult(objectToSerialize);
                    break;
                case "json":
                    result = new JsonResult(objectToSerialize);
                    break;
                default:
                    throw new ArgumentException("The requested format is not valid - " + format);
            }
            
            return result;
        }
    }
}
