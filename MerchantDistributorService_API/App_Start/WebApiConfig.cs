using Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MerchantDistributorService_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            //var cors = new EnableCorsAttribute("*", "accept,content-type,origin,x-my-header,Access-Control-Allow-Credentials,Access-Control-Allow-Origin,X-CSRF-Token,X-Requested-With,Accept-Version,Content-Length,Content-MD5,Date,X-Api-Version,X-File-Name", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            //config.EnableCors();
            config.Filters.Add(new ExceptionAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}
