using DataAccessLayer;
using Entities;
using Interface;
using Repository;
using System;
using System.Data.SqlClient;

namespace Repository
{
    public class CartRepository : ICart
    {
        public ServiceRes AddToCart(Cart cart)
        {
            ServiceRes serviceRes = new ServiceRes();
            try {
                SqlParameter[] sqlParameter = new SqlParameter[5];
                sqlParameter[0] = new SqlParameter { ParameterName = "@Product_Id", Value = cart.ProductId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@Member_Id", Value = cart.UserId };
                sqlParameter[2] = new SqlParameter { ParameterName = "@Quantity", Value = cart.Quantity }; 
                 sqlParameter[3] = new SqlParameter { ParameterName = "@Flag", Value = "A" };
                sqlParameter[4] = new SqlParameter { ParameterName = "@IsActive", Value = "1" };
                int returnValue = SqlHelper.ExecuteNonQuery("Usp_Cart", sqlParameter);
                if(returnValue < 0)
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Success";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Failed";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes DeleteItemfromCart(Cart cart)
        {
            throw new System.NotImplementedException();
        }

        public ServiceRes OrderConfirmation(OrderConfirmation orderConfirmation)
        {
            throw new NotImplementedException();
        }

        public ServiceRes ViewCartItem()
        {
            throw new NotImplementedException();
        }
    }
}
