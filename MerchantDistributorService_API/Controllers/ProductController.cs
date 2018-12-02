using Repository;
using Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;

namespace MerchantDistributorService_API.Controllers
{
    //[CustomAuthorize]
    public class ProductController : ApiController
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpPost, DeflateCompression,Cache(TimeDuration =10)]
        public IHttpActionResult GetProductList(Distributor_User request)
        {
                    return Ok(_productRepository.ViewProducts(request));
        }

        [HttpPost]
        public IHttpActionResult AddProduct(ProductDetails request)
        {
            return Ok(_productRepository.AddProduct(request));
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

        [HttpPost]
        public IHttpActionResult DistributorReport(Distributor_User request)
        {
            return Ok(this._productRepository.Distributor_Report(request));
        }

        [HttpPost]
        public IHttpActionResult GetAllRecentOrders(RecentReport request)
        {
            return Ok(this._productRepository.GetAllRecentOrders(request));
        }

        [HttpPost]
        public IHttpActionResult RecentOrdersById(RecentReport request)
        {
            return Ok(this._productRepository.RecentOrderDetail(request));
        }

        [HttpPost]
        public IHttpActionResult Distributor_ConfirmOrder(ConfirmOrder request)
        {
            return Ok(this._productRepository.Distributor_ConfirmOrder(request));
        }

    }
}
