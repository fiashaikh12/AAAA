using System;
using System.Collections.Generic;
using Repository;
using Entities;
using DataAccessLayer;
using System.Data;
using static Enum.Enums;
using System.Data.SqlClient;

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
                    SqlParameter[] sqlParameters = new SqlParameter[9];
                    sqlParameters[0] = new SqlParameter { ParameterName="",Value=objProduct.Name };
                    sqlParameters[1] = new SqlParameter { ParameterName = "", Value = objProduct.SKUNumber };
                    sqlParameters[2] = new SqlParameter { ParameterName = "", Value = objProduct.Specification };
                    sqlParameters[3] = new SqlParameter { ParameterName = "", Value = objProduct.Price };
                    sqlParameters[4] = new SqlParameter { ParameterName = "", Value = objProduct.IsPackaging };
                    sqlParameters[5] = new SqlParameter { ParameterName = "", Value = objProduct.IsAvailable };
                    sqlParameters[6] = new SqlParameter { ParameterName = "", Value = objProduct.ImagePath };
                    sqlParameters[7] = new SqlParameter { ParameterName = "", Value = objProduct.Discount };
                    sqlParameters[8] = new SqlParameter { ParameterName = "", Value = objProduct.Category};
                    int returnValue = SqlHelper.ExecuteNonQuery("", sqlParameters);
                    if(returnValue > 0)
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
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
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

        public ServiceRes GetAllProductDetails()
        {
            ServiceRes<List<ProductDetails>> serviceRes = new ServiceRes<List<ProductDetails>>();
            List<ProductDetails> productDetails = new List<ProductDetails>();
            try {
                var dt = SqlHelper.GetTableFromSP("USP_GET_ALL_PRODUCTS");
                if(dt!=null && dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        productDetails.Add(new ProductDetails
                        {
                            Name = Convert.ToString(row[""]),
                            EmailAddress = Convert.ToString(row[""]),
                            Mobile = Convert.ToString(row[""])
                        });
                    }
                    serviceRes.Data = productDetails;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "OK";                       
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, Enum.Enums.SeverityLevel.Important);
            }
            return serviceRes;
        }

        public ServiceRes SalesReport()
        {
            throw new NotImplementedException();
        }

        public ServiceRes UpdateProduct(ProductDetails objProduct)
        {
            throw new NotImplementedException();
        }
    }
}
