using Entities;
using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class CartController : ApiController
    {
        private ICart _cart;
        public CartController(ICart cart)
        {
            this._cart = cart;
        }

        [HttpPost]
        public IHttpActionResult AddtoCart(Cart request)
        {
            return Ok(_cart.AddToCart(request));
        }
        [HttpPost]
        public IHttpActionResult OrderConfirmation(OrderConfirmation request)
        {
            return Ok(_cart.OrderConfirmation(request));
        }
    }
}
