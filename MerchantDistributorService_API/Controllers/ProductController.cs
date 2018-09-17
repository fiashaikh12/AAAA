using Repository;
using Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class ProductController : ApiController
    {
        private IProductRepository _productRepository;
        public ProductController()
        {
            this._productRepository = new ProductRepository();
        }
        [HttpPost, DeflateCompression,Cache(TimeDuration =10)]
        public HttpResponseMessage GetProductList()
        {
                    return Request.CreateResponse(HttpStatusCode.OK, _productRepository.GetAllProductDetails());
        }
    }
}
