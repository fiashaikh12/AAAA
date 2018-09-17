using Repository;
using System;
using System.Linq;

namespace MerchantDistributorService_API.Common
{
    public class TokenGenerator
    {
        public  static string GenerateToken()
        {
            var encrypt = CryptographyRepository.GetInstance;
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            encrypt.Encrypt(token);
            return token.ToString();
        }
        public static bool IsTokenValid(string accessToken)
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