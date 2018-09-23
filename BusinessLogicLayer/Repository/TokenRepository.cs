using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class TokenRepository : IToken
    {
        private static readonly object padlock = new object();
        private static TokenRepository _instance = null;
        public static TokenRepository GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TokenRepository();
                        }
                    }
                }
                return _instance;
            }
        }

        public string GenerateToken()
        {
            var encrypt = CryptographyRepository.GetInstance;
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            encrypt.Encrypt(token);
            return token.ToString();
        }

        public bool IsTokenValid(string accessToken)
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
