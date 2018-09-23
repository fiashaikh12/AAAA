using Entities;
using MerchantDistributorService_API.Common;
using Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;

/*Developed by Arif & Firoz*/
namespace MerchantDistributorService_API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommonRepository _commonRepository;

        public UserController(IUserRepository userRepo, ICommonRepository commonRepo)
        {
            this._userRepository = userRepo;
            this._commonRepository = commonRepo;
        }

        [HttpPost]
        public IHttpActionResult ValidateUser(User request)
        {
                return Ok(this._userRepository.IsUserValid(request));
        }

        [HttpPost]
        public IHttpActionResult RegisterUser(Registration request)
        {
            return Ok(this._userRepository.RegisterUser(request));
        }

        [HttpGet]
        public HttpResponseMessage GetStates()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this._commonRepository.GetStates());
        }

        [HttpGet]
        public HttpResponseMessage GetGender()
        {
            return Request.CreateResponse(HttpStatusCode.OK, this._commonRepository.GetGenders());
        }

        [HttpPost]
        public HttpResponseMessage GetCityByState(States request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this._commonRepository.GetCitiesByState(request));
        }

    }
}
