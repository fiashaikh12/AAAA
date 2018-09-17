using AutoFactory;
using Repository;
using System;
using System.Web;
using System.Web.Http;

namespace MerchantDistributorService_API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Run();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            // Handle HTTP errors
            LogManager.WriteLog(exc, Enum.Enums.SeverityLevel.Important);
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //    var cors = new EnableCorsAttribute("*", "*", "*");
            ////    config.EnableCors(cors);
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //These headers are handling the "pre-flight" OPTIONS call sent by the browser
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Access-Control-Allow-Origin, X-Requested-With, Soapaction,cache-control,postman-token");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
    }
}
