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
        public IHttpActionResult DeliveryTimeSlot()
        {
            return Ok(_cart.DeliveryTimeSlot());
        }

        [HttpPost]
        public IHttpActionResult ViewCart(Cart request)
        {
            return Ok(_cart.ViewCartItem(request));
        }
        [HttpPost]
        public IHttpActionResult DeleteItemFromCart(Cart request)
        {
            return Ok(_cart.DeleteItemfromCart(request));
        }

        [HttpPost]
        public IHttpActionResult OrderConfirmation(Order request)
        {
            return Ok(_cart.OrderConfirmation(request));
        }
    }
}
