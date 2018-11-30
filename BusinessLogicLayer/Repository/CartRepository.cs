using DataAccessLayer;
using Entities;
using Interface;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository
{
    public class CartRepository : ICart
    {
        public ServiceRes AddToCart(Cart cart)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[5];
                sqlParameter[0] = new SqlParameter { ParameterName = "@Product_Id", Value = cart.ProductId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@Member_Id", Value = cart.UserId };
                sqlParameter[2] = new SqlParameter { ParameterName = "@Quantity", Value = cart.Quantity };
                sqlParameter[3] = new SqlParameter { ParameterName = "@Flag", Value = "A" };
                sqlParameter[4] = new SqlParameter { ParameterName = "@IsActive", Value = "1" };
                int returnValue = SqlHelper.ExecuteNonQuery("Usp_Cart", sqlParameter);
                if (returnValue > 0)
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
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter { ParameterName = "@Member_Id", Value = cart.UserId };
                sqlParameters[1] = new SqlParameter { ParameterName = "@Product_Id", Value = cart.ProductId };
                sqlParameters[2] = new SqlParameter { ParameterName = "@Flag", Value = "D" };
                int ret = SqlHelper.ExecuteNonQuery("Usp_Cart", sqlParameters);
                if (ret > 0)
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Success";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "No item in bag";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes OrderConfirmation(Order orderConfirmation)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                if (orderConfirmation != null)
                {

                    List<int> orderId = new List<int>();
                    int userId = 0;
                    int OrderId = 0;
                    foreach (var items in orderConfirmation.Request)
                    {
                        DataTable dtOrderId = new DataTable();
                        if (userId != items.DistributorId)
                        {
                            userId = items.DistributorId;
                            string OrderNumber = $"DO{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                            SqlParameter[] sqlParameter = new SqlParameter[6];
                            sqlParameter[0] = new SqlParameter { ParameterName = "@Order_Number", Value = OrderNumber };
                            sqlParameter[1] = new SqlParameter { ParameterName = "@Order_Date", Value = DateTime.Now };
                            sqlParameter[2] = new SqlParameter { ParameterName = "@MemberId", Value = items.DistributorId };
                            sqlParameter[3] = new SqlParameter { ParameterName = "@Total_Amount", Value = items.TotalAmount };
                            sqlParameter[4] = new SqlParameter { ParameterName = "@Delivery_TimeSlot_Id", Value = items.DeliveryTimeSlotId };
                            sqlParameter[5] = new SqlParameter { ParameterName = "@Flag", Value = "OM" };
                            dtOrderId = SqlHelper.GetTableFromSP("USP_OrderConfirmation", sqlParameter);
                            if (dtOrderId.Rows.Count > 0)
                            {
                                OrderId = Convert.ToInt32(dtOrderId.Rows[0][0]);
                            }
                        }
                        orderId.Add(OrderId);
                    }
                    foreach (var item in orderConfirmation.Request)
                    {
                        DataTable dtOrderId = new DataTable();
                        foreach (var id in orderId)
                        {
                            SqlParameter[] sqlParameter = new SqlParameter[7];
                            sqlParameter[0] = new SqlParameter { ParameterName = "@Order_Id", Value = id };
                            sqlParameter[1] = new SqlParameter { ParameterName = "@Product_Id", Value = item.ProductId };
                            sqlParameter[2] = new SqlParameter { ParameterName = "@Quantity", Value = item.Quantity };
                            sqlParameter[3] = new SqlParameter { ParameterName = "@MemberId", Value = item.RetailerId };
                            sqlParameter[4] = new SqlParameter { ParameterName = "@Amount", Value = item.Amount };
                            sqlParameter[5] = new SqlParameter { ParameterName = "@Payment_Mode", Value = "COD" };
                            sqlParameter[6] = new SqlParameter { ParameterName = "@Flag", Value = "OD" };
                            dtOrderId = SqlHelper.GetTableFromSP("USP_OrderConfirmation", sqlParameter);
                        }
                        if (dtOrderId.Rows.Count > 0)
                        {
                            serviceRes.IsSuccess = true;
                            serviceRes.ReturnCode = "200";
                            serviceRes.ReturnMsg = "Success";
                        }
                        else
                        {
                            serviceRes.IsSuccess = false;
                            serviceRes.ReturnCode = "400";
                            serviceRes.ReturnMsg = "failed";
                        }
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes DeliveryTimeSlot()
        {
            ServiceRes<List<OrderDeliveryTimeSlot>> serviceRes = new ServiceRes<List<OrderDeliveryTimeSlot>>();
            try
            {
                DataTable dataTable = SqlHelper.GetTableFromSP("Usp_GetDeliveryTimeSlot");
                serviceRes.Data = dataTable.AsEnumerable().Select(x =>
                    new OrderDeliveryTimeSlot
                    {
                        TimeSlotId = x.Field<int>("Delivery_TimeSlot_ID"),
                        TimeSlotText = x.Field<string>("Delivery_TimeText")
                    }).ToList();
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "Success";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes ViewCartItem(Cart cart)
        {
            ServiceRes<List<ProductListByCategory>> serviceRes = new ServiceRes<List<ProductListByCategory>>();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter { ParameterName = "@Member_Id", Value = cart.UserId };
                sqlParameters[1] = new SqlParameter { ParameterName = "@Flag", Value = "S" };
                DataTable dataTable = SqlHelper.GetTableFromSP("Usp_Cart", sqlParameters);
                if (dataTable.Rows.Count > 0)
                {
                    List<ProductListByCategory> productDetails = dataTable.AsEnumerable().
                        Select(x => new ProductListByCategory
                        {
                            Price = x.Field<decimal>("Price"),
                            Specification = x.Field<string>("Specification"),
                            Quantity = x.Field<int>("Quantity"),
                            Photos_Url = $"http://escandent.com/{x.Field<string>("Photos_Url")}",
                            Name = x.Field<string>("Name"),
                            ProductId = x.Field<int>("Product_Id")
                        }).ToList();
                    serviceRes.Data = productDetails;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Success";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "No item in bag";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }
    }
}
