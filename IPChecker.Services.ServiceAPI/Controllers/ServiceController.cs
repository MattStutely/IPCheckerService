using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using log4net;
using IPChecker.Services.Business.Services.Interfaces;
using IPChecker.Services.ServiceAPI.Utility.Results;

namespace IPChecker.Services.ServiceAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET,POST")]
    public class ServiceController : ApiController
    {
        private IIPService _service;
        private readonly ILog _log = LogManager.GetLogger(typeof (ServiceController));

        public ServiceController(IIPService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("validateip.{format}")]
        public HttpResponseMessage ValidateIP(string format)
        {
            try
            {
                return ResultFactory.CreateResult(_service.ValidateIP(), format);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
