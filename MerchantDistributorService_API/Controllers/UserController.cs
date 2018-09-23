﻿using Entities;
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
        public IHttpActionResult GetStates()
        {
            return Ok(this._commonRepository.GetStates());
        }

        [HttpGet]
        public IHttpActionResult GetGender()
        {
            return Ok(this._commonRepository.GetGenders());
        }

        [HttpPost]
        public IHttpActionResult GetCityByState(States request)
        {
            return Ok(this._commonRepository.GetCitiesByState(request));
        }

        [HttpPost]
        public IHttpActionResult GetBusinessCategoryMaster()
        {
            return Ok(this._commonRepository.GetBusinessCatMaster());
        }
    }
}
