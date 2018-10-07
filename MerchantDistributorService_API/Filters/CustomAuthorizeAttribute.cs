using Entities;
using Repository;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly Token _token;
        private readonly TokenRepository _instance;
        public CustomAuthorizeAttribute()
        {
            this._token = new Token();
            this._instance = TokenRepository.GetInstance;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            _token.AccessToken = ((string[])actionContext.Request.Headers.GetValues("Token"))[0];
            _token.UserId =((string[])actionContext.Request.Headers.GetValues("UserId"))[0];
            if (!_instance.IsTokenValid(_token))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
        }
    }
}