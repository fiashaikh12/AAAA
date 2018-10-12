using BusinessObjects.Entities;
using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class DashboardController : ApiController
    {
        private readonly IDashboard _dashboardRepository;
        public DashboardController(IDashboard dashboard) {
            this._dashboardRepository = dashboard;
        }

        [HttpPost]
        public IHttpActionResult NearByDistributor(NearByDistributors request)
        {
            return Ok(_dashboardRepository.NearByDistributors(request));
        }
    }
}
