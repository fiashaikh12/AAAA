using Entities;
using Repository;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdmin _adminRepository;
        public AdminController(IAdmin admin)
        {
            this._adminRepository = admin;
        }

        [HttpPost]
        public IHttpActionResult DeleteCategory(ProductCategory request)
        {
            return Ok(_adminRepository.DeleteCategory(request.CategoryId));
        }

        [HttpPost]
        public IHttpActionResult CreateCategory(ProductCategory request)
        {
            return Ok(_adminRepository.AddCategory(request.Name));
        }

        [HttpPost]
        public IHttpActionResult UpdateCategory(ProductCategory request)
        {
            return Ok(_adminRepository.UpdateCategory(request.CategoryId, request.Name));
        }

        [HttpPost]
        public IHttpActionResult ValidateAdmin(AdminRequest request)
        {
            return Ok(_adminRepository.IsAdminValid(request));
        }
    }
}
