using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
namespace MerchantDistributorService_API.Controllers
{
    public class MessageController : ApiController
    {
        private readonly IMessage _message;
        public MessageController(IMessage message)
        {
            this._message = message;
        }

        [HttpPost]
        public IHttpActionResult Send(Messages request)
        {
            return Ok(this._message.Send(request));
        }

        [HttpPost]
        public IHttpActionResult GetMessages(Messages request)
        {
            return Ok(this._message.AllMessages(request));
        }
    }
}
