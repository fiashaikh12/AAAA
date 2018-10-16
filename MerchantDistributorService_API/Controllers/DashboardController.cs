using Entities;
using Interface;
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

        [HttpPost]
        public IHttpActionResult CategoryListByDistributor(NearByDistributors request)
        {
            return Ok(_dashboardRepository.CategoryListByDistributor(request));
        }

        [HttpPost]
        public IHttpActionResult ProductListByCategory(NearByDistributors request)
        {
            return Ok(this._dashboardRepository.ProductByCategories(request));
        }
    }
}
