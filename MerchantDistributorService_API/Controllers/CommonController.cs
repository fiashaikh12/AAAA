using Entities;
using Filters;
using Repository;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class CommonController : ApiController
    {
        private readonly ICommonRepository _commonRepository;
        public CommonController(ICommonRepository commonRepo)
        {
            this._commonRepository = commonRepo;
        }
        [HttpGet]
        public IHttpActionResult GetStates()
        {
            return Ok(this._commonRepository.GetStates());
        }

        [HttpGet]
        public IHttpActionResult GetGender()
        {
            return Ok(this._commonRepository.GetGenders());
        }

        [HttpPost]
        public IHttpActionResult GetCityByState(States request)
        {
            return Ok(this._commonRepository.GetCitiesByState(request));
        }
        [HttpPost]
        public IHttpActionResult GetBusinessMaster()
        {
            return Ok(this._commonRepository.GetBusinessType());
        }
    }
}
