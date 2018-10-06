using Repository;
using Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;

namespace MerchantDistributorService_API.Controllers
{
    [CustomAuthorize]
    public class ProductController : ApiController
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        [HttpPost, DeflateCompression,Cache(TimeDuration =10)]
        public IHttpActionResult GetProductList()
        {
                    return Ok(_productRepository.GetAllProductDetails());
        }
        [HttpPost]
        public IHttpActionResult AddProduct(ProductDetails productDetails)
        {
            return Ok(_productRepository.AddProduct(productDetails));
        }
        [HttpPost]
        public IHttpActionResult GetCategoryMaster()
        {
            return Ok(this._productRepository.GetCategoryMaster());
        }

        [HttpPost]
        public IHttpActionResult GetSubCategoryMaster(ProductCategory request)
        {
            return Ok(this._productRepository.GetSubCategoryMaster(request.CategoryId));
        }
    }
}
