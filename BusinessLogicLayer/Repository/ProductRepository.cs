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
                    SqlParameter[] sqlParameters = new SqlParameter[14];
                    sqlParameters[0] = new SqlParameter { ParameterName = "@Product_Name", Value = objProduct.Name };
                    sqlParameters[1] = new SqlParameter { ParameterName = "@SKUNumber", Value = objProduct.SKUNumber };
                    sqlParameters[2] = new SqlParameter { ParameterName = "@Product_Description", Value = objProduct.Specification };
                    sqlParameters[3] = new SqlParameter { ParameterName = "@Price", Value = objProduct.Price };
                    sqlParameters[4] = new SqlParameter { ParameterName = "@IsPackaging", Value = objProduct.IsPackaging };
                    sqlParameters[5] = new SqlParameter { ParameterName = "@IsAvailable", Value = objProduct.IsAvailable };
                    sqlParameters[6] = new SqlParameter { ParameterName = "@Photos_Url", Value = objProduct.ImagePath };
                    sqlParameters[7] = new SqlParameter { ParameterName = "@Discount", Value = objProduct.Discount };
                    sqlParameters[8] = new SqlParameter { ParameterName = "@Product_Category_Id", Value = objProduct.CategoryId };
                    sqlParameters[9] = new SqlParameter { ParameterName = "@Product_SubCategory_Id", Value = objProduct.SubCategoryId };
                    sqlParameters[10] = new SqlParameter { ParameterName = "@Quantity", Value = objProduct.Quantity };
                    sqlParameters[11] = new SqlParameter { ParameterName = "@Photos_Url", Value = objProduct.ImagePath};
                    sqlParameters[12] = new SqlParameter { ParameterName = "@Member_Id", Value = objProduct.UserId };
                    sqlParameters[13] = new SqlParameter { ParameterName = "@flag", Value = "A" };
                    int returnValue = SqlHelper.ExecuteNonQuery("Usp_Products", sqlParameters);
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
            try
            {
                var dt = SqlHelper.GetTableFromSP("USP_GET_ALL_PRODUCTS");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        productDetails.Add(new ProductDetails
                        {
                            //Name = Convert.ToString(row[""]),
                            //EmailAddress = Convert.ToString(row[""]),
                            //Mobile = Convert.ToString(row[""])
                        });
                    }
                    serviceRes.Data = productDetails;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "OK";
                }
            }
            catch (Exception ex)
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
                LogManager.WriteLog(ex, SeverityLevel.Critical);
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
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return serviceRes;
        }
    }
}
