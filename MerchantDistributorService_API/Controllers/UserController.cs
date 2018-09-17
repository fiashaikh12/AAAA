using Entities;
using MerchantDistributorService_API.Common;
using Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public HttpResponseMessage ValidateUser(User request)
        {
            if (TokenGenerator.IsTokenValid(request.AccessToken))
            {
                return Request.CreateResponse(HttpStatusCode.OK, this._userRepository.IsUserValid(request));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid token and login attempt");
            }
        }

        [HttpPost]
        public HttpResponseMessage RegisterUser(Registration request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, this._userRepository.RegisterUser(request));
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
