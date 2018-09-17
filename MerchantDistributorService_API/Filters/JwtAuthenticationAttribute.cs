using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Filters
{
    public class JwtAuthenticationAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //if (!IsUserAuthorized(actionContext))
            //{
            //    ShowAuthenticationError(actionContext);
            //    return;
            //}
            base.OnAuthorization(actionContext);
        }
    }
}