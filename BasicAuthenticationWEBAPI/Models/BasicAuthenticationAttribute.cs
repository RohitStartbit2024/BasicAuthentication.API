using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace BasicAuthenticationWEBAPI.Models
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        private const string Realm = "My Realm";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.ContainsKey("Authorization"))
            {
                context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{Realm}\"");
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            var authorizationHeader = request.Headers["Authorization"].ToString();
            var authToken = authorizationHeader.Split(" ")[1]; 
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var username = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];

            if (UserValidate.Login(username, password))
            {
                var identity = new GenericIdentity(username);
                var principal = new GenericPrincipal(identity, null);

                context.HttpContext.User = principal;
                Thread.CurrentPrincipal = principal; 
            }
            else
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}
