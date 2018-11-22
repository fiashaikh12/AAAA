using Repository;
using System;
using System.Linq;
using System.Web.Http;

namespace MerchantDistributorService_API.Controllers
{
    public class BaseController : ApiController
    {
        protected readonly IUserRepository _userRepository;
        protected readonly ICommonRepository _commonRepository;
        protected readonly IProductRepository _productRepository;
        public BaseController(IUserRepository userRepository, ICommonRepository commonRepository,IProductRepository _productRepository)
        {
            this._userRepository = userRepository;
            this._commonRepository = commonRepository;
        }
        

        private string GenerateToken()
        {
            var encrypt = CryptographyRepository.GetInstance;
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            encrypt.Encrypt(token);
            return token.ToString();
        }
        private bool IsTokenValid(string accessToken)
        {
            var IsValid = true;
            var decrypt = CryptographyRepository.GetInstance;
            accessToken = decrypt.Decrypt(accessToken);
            byte[] data = Convert.FromBase64String(accessToken);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                IsValid = false;
            }
            return IsValid;
        }
    }
}
