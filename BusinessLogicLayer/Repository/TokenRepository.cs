﻿using DataAccessLayer;
using Entities;
using System;
using System.Data.SqlClient;
using System.Linq;

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

        public Token GenerateToken(int userId)
        {
            Token token = new Token();
            //var encrypt = CryptographyRepository.GetInstance;
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            token.UserId =Convert.ToString(userId);
            token.AccessToken = Convert.ToBase64String(time.Concat(key).ToArray());
            //encrypt.Encrypt(token.AccessToken);

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter { ParameterName = "@memberId", Value = token.UserId };
            sqlParameter[1] = new SqlParameter { ParameterName = "@accessToken", Value = token.AccessToken };
            int returnValue = SqlHelper.ExecuteNonQuery("Usp_Add_Token", sqlParameter);
            return token;
        }

        public bool IsTokenValid(Token token)
        {
            var IsValid = true;
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter { ParameterName = "@memberId", Value = token.UserId };
            var tokenResponse = SqlHelper.ExecuteNonQuery("Usp_Get_Token", sqlParameter);

            byte[] data = Convert.FromBase64String(token.AccessToken);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-2))
            {
                IsValid = false;
            }
            return IsValid;
        }
    }
}
