using Entities;
using Filters;
using Repository;
using System.Web.Http;

/*Developed by Arif & Firoz*/
namespace MerchantDistributorService_API.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;
       
        public UserController(IUserRepository userRepo)
        {
            this._userRepository = userRepo;           
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
    }
}
