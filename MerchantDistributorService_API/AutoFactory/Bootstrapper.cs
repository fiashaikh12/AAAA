using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AutoFactory
{
    public class Bootstrapper
    {
        public static void Run()
        {
            AutoFactoryConfiguration.Initialize(GlobalConfiguration.Configuration);
        }
    }
}