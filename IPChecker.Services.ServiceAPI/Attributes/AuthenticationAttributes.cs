using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IPChecker.Services.ServiceAPI.Attributes
{
    //This first class sets up generic authenication based on HTTP basic authentication, a base64 username:password is sent from the client
    //this is decoded and checked against either a database or the web.config file, you'll need to configure it
    //if acceptable the method will be allowed, otherwise it will return a 401:Unauthorized
    //to use just decorate a controller method with [BasicAuthentication]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthentication : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = ParseAuthorizationHeader(actionContext);
            if (identity == null)
            {
                DenyAccess(actionContext);
                return;
            }


            if (!IsUserAuthorised(identity.Name, identity.Password, actionContext))
            {
                DenyAccess(actionContext);
                return;
            }

            var principal = new GenericPrincipal(identity, null);

            Thread.CurrentPrincipal = principal;

            base.OnAuthorization(actionContext);
            
        }

        protected virtual bool IsUserAuthorised(string username, string password, HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            //TODO:check username and password against your web.config or database and return true or false 
            return false;
        }

        private BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }

        protected void DenyAccess(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

    }

    //This class manages the identity of the user and should not be changed
    internal class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }

        public string Password { get; set; }
    }

    //Finally you can extend the basic auth class to instead have multiple authentications for different controller methods
    //e.g one for getting data, one for posting data
    //if you want more, use the template below to create as many as desired
    //then just decorate the controller method with [YourFilterName]
    //public class BespokeAuthentication : BasicAuthentication
    //{

    //    protected override bool IsUserAuthorised(string username, string password, HttpActionContext actionContext)
    //    {
    //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    //            return false;

    //        //custom verification code here
    //        return false;
    //    }
    
    //}

    

    
}
