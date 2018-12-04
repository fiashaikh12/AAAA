using System;
using System.Collections.Generic;
using Entities;
using DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository
{
    public class ProductRepository : IProductRepository

    {
        public ServiceRes AddProduct(ProductDetails objProduct)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                if (objProduct != null)
                {
                    ICommonRepository _commonRepository = new CommonRepository();
                    string fileLocation = _commonRepository.Base64toImage(objProduct.ImagePath, "Images", "ProductImages","ProductPhoto");
                    SqlParameter[] sqlParameters = new SqlParameter[13];
                    sqlParameters[0] = new SqlParameter { ParameterName = "@Name", Value = objProduct.Name };
                    sqlParameters[1] = new SqlParameter { ParameterName = "@SKUNumber", Value = objProduct.SKUNumber };
                    sqlParameters[2] = new SqlParameter { ParameterName = "@Specification", Value = objProduct.Specification };
                    sqlParameters[3] = new SqlParameter { ParameterName = "@Price", Value = objProduct.Price };
                    sqlParameters[4] = new SqlParameter { ParameterName = "@IsPackaging", Value = objProduct.IsPackaging };
                    sqlParameters[5] = new SqlParameter { ParameterName = "@IsAvailable", Value = objProduct.IsAvailable };
                    sqlParameters[6] = new SqlParameter { ParameterName = "@Discount", Value = objProduct.Discount };
                    sqlParameters[7] = new SqlParameter { ParameterName = "@Category_Id", Value = objProduct.CategoryId };
                    sqlParameters[8] = new SqlParameter { ParameterName = "@SubCategory_Id", Value = objProduct.SubCategoryId };
                    sqlParameters[9] = new SqlParameter { ParameterName = "@Quantity", Value = objProduct.Quantity };
                    sqlParameters[10] = new SqlParameter { ParameterName = "@Photos_Url", Value = fileLocation };
                    sqlParameters[11] = new SqlParameter { ParameterName = "@Member_Id", Value = objProduct.UserId };
                    sqlParameters[12] = new SqlParameter { ParameterName = "@flag", Value = "A" };
                    int returnValue = SqlHelper.ExecuteNonQuery("Usp_Products_Test", sqlParameters);
                    if (returnValue > 0)
                    {
                        serviceRes.IsSuccess = true;
                        serviceRes.ReturnCode = "200";
                        serviceRes.ReturnMsg = "Product added successfully";
                    }
                    else
                    {
                        serviceRes.IsSuccess = false;
                        serviceRes.ReturnCode = "400";
                        serviceRes.ReturnMsg = "Something went wrong";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "400";
                serviceRes.ReturnMsg = "Error occured in database";
            }
            return serviceRes;
        }

        public ServiceRes DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public ServiceRes ViewProducts(Distributor_User distributor_User)
        {
            ServiceRes<List<ProductListByCategory>> serviceRes = new ServiceRes<List<ProductListByCategory>>();
            List<ProductDetails> productDetails = new List<ProductDetails>();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter { ParameterName = "@distributorId", Value = distributor_User.UserId };
                var dataTable = SqlHelper.GetTableFromSP("Usp_DistributorProducts", sqlParameters);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {

                    serviceRes.Data = dataTable.AsEnumerable().Select(x=>                    
                    new ProductListByCategory
                    {
                        Name=x.Field<string>("ProductName"),
                        Quantity=x.Field<int>("Quantity"),
                        Price=x.Field<decimal>("Price"),
                        Photos_Url=x.Field<string>("Photos_Url"),
                        Specification=x.Field<string>("Specification")
                    }).ToList();
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Product list ";
                }
                else
                {
                    serviceRes.Data = null;
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "202";
                    serviceRes.ReturnMsg = "Product not found";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes UpdateProduct(ProductDetails objProduct)
        {
            throw new NotImplementedException();
        }

        public ServiceRes GetSubCategoryMaster(int categoryId)
        {
            ServiceRes<List<ProductSubCategory>> serviceRes = new ServiceRes<List<ProductSubCategory>>();
            try
            {
                List<ProductSubCategory> subCategories = new List<ProductSubCategory>();
                SqlParameter[] sqlParameter = new SqlParameter[1];
                sqlParameter[0] = new SqlParameter { ParameterName = "@categoryId", Value = categoryId };
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetSubCategory", sqlParameter);
                foreach (DataRow row in dtCities.Rows)
                {
                    ProductSubCategory businessCategory = new ProductSubCategory
                    {
                        SubCategoryId = Convert.ToInt32(row["Product_SubCategoryId"]),
                        Name = Convert.ToString(row["SubCategory_Name"])
                    };
                    subCategories.Add(businessCategory);
                }
                serviceRes.Data = subCategories;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "Sub Category master";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes GetCategoryMaster()
        {
            ServiceRes<List<ProductCategory>> serviceRes = new ServiceRes<List<ProductCategory>>();
            try
            {
                List<ProductCategory> productCategories = new List<ProductCategory>();
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetCategory");
                foreach (DataRow row in dtCities.Rows)
                {
                    ProductCategory productCategory = new ProductCategory
                    {
                        CategoryId = Convert.ToInt32(row["Product_Category_Id"]),
                        Name = Convert.ToString(row["Product_Name"])
                    };
                    productCategories.Add(productCategory);
                }
                serviceRes.Data = productCategories;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "Category master";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes GetAllRecentOrders(RecentReport recentReport)
        {
            ServiceRes<List<RecentReport>> serviceRes = new ServiceRes<List<RecentReport>>();
            try {
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter { ParameterName = "@MemberId", Value = recentReport.UserId };
                sqlParameters[1] = new SqlParameter { ParameterName = "@Flag", Value = "OL" };

                var dataTable = SqlHelper.GetTableFromSP("USP_DistributorOrder", sqlParameters);
                if(dataTable.Rows.Count > 0) {
                    serviceRes.Data = dataTable.AsEnumerable().Select(x => new RecentReport {
                        OrderId=x.Field<int>("Order_Id"),
                        OrderNumber=x.Field<string>("Order_Number"),
                        OrderDate=x.Field<DateTime>("Order_Date"),
                        UserId=x.Field<int>("Member_Id"),
                        ProductId=x.Field<int>("Product_Id"),
                        MechantName=x.Field<string>("MerchantName"),
                        Locality=x.Field<string>("Locality"),
                        PinCode=Convert.ToString(x.Field<int>("PinCode")),
                        EmailAddress=x.Field<string>("Email")
                    }).ToList();
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
            }
            catch(Exception ex)
            {
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Exception Occured";
                LogManager.WriteLog(ex);
            }
           return  serviceRes;
        }

        public ServiceRes RecentOrderDetail(RecentReport recentReport)
        {
            ServiceRes<List<RecentReport>> serviceRes = new ServiceRes<List<RecentReport>>();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter { ParameterName = "@MemberId", Value = recentReport.UserId };
                sqlParameters[1] = new SqlParameter { ParameterName = "@Order_Id", Value = recentReport.OrderId };
                sqlParameters[2] = new SqlParameter { ParameterName = "@Flag", Value = "OP" };

                var dataTable = SqlHelper.GetTableFromSP("USP_DistributorOrder", sqlParameters);
                if (dataTable.Rows.Count > 0)
                {
                    serviceRes.Data = dataTable.AsEnumerable().Select(x => new RecentReport
                    {
                        OrderId = x.Field<int>("Order_Id"),
                        OrderNumber = x.Field<string>("Order_Number"),
                        OrderDate = x.Field<DateTime>("Order_Date"),
                        UserId = x.Field<int>("Member_Id"),
                        ProductId = x.Field<int>("Product_Id"),
                        OrderQuantity=x.Field<int>("OQty"),
                        DeliveredQuantity=x.Field<int>("DQty"),
                        CreatedOn=x.Field<DateTime>("Created_On"),
                        ProductName=x.Field<string>("ProductName"),
                        MechantName = x.Field<string>("MerchantName"),
                        Price=x.Field<decimal>("Price")
                    }).ToList();
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
            }
            catch (Exception ex)
            {
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Exception Occured";
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes Distributor_Report(Distributor_User distributor_User)
        {
            ServiceRes<DataSet> serviceRes = new ServiceRes<DataSet>();
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter { ParameterName = "@MemberId", Value = distributor_User.UserId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@Date", Value = distributor_User.FilterDate == null ? DateTime.Now: distributor_User.FilterDate };
                var dataSet = SqlHelper.GetDatasetFromSP("USP_DistributorReport", sqlParameter);
                if (dataSet.Tables.Count > 0)
                {
                    serviceRes.Data = dataSet;
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

;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes Distributor_ConfirmOrder(ConfirmOrder  confirmOrder)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                if (confirmOrder.Request != null)
                {
                    int retValue = 0;
                    foreach (var order in confirmOrder.Request)
                    {
                        SqlParameter[] sqlParameter = new SqlParameter[5];
                        sqlParameter[0] = new SqlParameter { ParameterName = "@MemberId", Value = order.UserId };
                        sqlParameter[1] = new SqlParameter { ParameterName = "@Order_Id", Value = order.OrderId };
                        sqlParameter[2] = new SqlParameter { ParameterName = "@Product_Id", Value = order.ProductId };
                        sqlParameter[3] = new SqlParameter { ParameterName = "@Order_Status", Value = "D" };
                        sqlParameter[4] = new SqlParameter { ParameterName = "@Flag", Value = "OS" };
                        retValue = SqlHelper.ExecuteNonQuery("USP_DistributorOrder", sqlParameter);
                        if (retValue > 0)
                        {
                            EmailerRepository emailerRepository = EmailerRepository.GetInstance;
                            if (emailerRepository.Send(order.EmailAddress))
                            {
                                serviceRes.IsSuccess = true;
                                serviceRes.ReturnCode = "200";
                                serviceRes.ReturnMsg = "Success";
                            }
                        }
                        else
                        {
                            serviceRes.IsSuccess = false;
                            serviceRes.ReturnCode = "400";
                            serviceRes.ReturnMsg = "Failed";
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "400";
                serviceRes.ReturnMsg = "Exception occurred.";
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }
    }
}
