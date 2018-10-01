using System.Web.Http;
using System.Web.Http.Controllers;

namespace Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string username = ((string[])actionContext.Request.Headers.GetValues("User"))[0];
            string password = ((string[])actionContext.Request.Headers.GetValues("Password"))[0];
            if(!Authenticate(username, password))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }

        private bool Authenticate(string username,string password)
        {
            bool isAuthenticated = false;
            if(username.Equals("firoz") && password.Equals("1234"))
            {
                isAuthenticated = true;
            }
            return isAuthenticated;
        }
    }
}