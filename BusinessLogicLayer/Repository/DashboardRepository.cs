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
    public class DashboardRepository : IDashboard
    {
        public ServiceRes CategoryListByDistributor(NearByDistributors byDistributors)
        {
            ServiceRes<List<ProductCategory>> serviceRes = new ServiceRes<List<ProductCategory>>();
            try {
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter { ParameterName = "@Member_Id", Value = byDistributors.UserId };
                sqlParameters[1] = new SqlParameter { ParameterName = "@flag", Value = "CL" };
                DataTable dtByDistributors = SqlHelper.GetTableFromSP("Usp_Products_Test", sqlParameters);
                if(dtByDistributors.Rows.Count > 0)
                {
                    List<ProductCategory> productCategories = dtByDistributors.AsEnumerable().
                        Select(x => new ProductCategory {
                            CategoryId=x.Field<int>("Product_Category_Id"),
                            Name=x.Field<string>("Product_Name"),
                            CompanyName=x.Field<string>("Company_Name"),
                            Building_Name=x.Field<string>("Building_Name"),
                            Locality=x.Field<string>("Locality"),
                            PinCode=Convert.ToInt32(x.Field<int>("PinCode")),
                            State=x.Field<string>("State"),
                            City=x.Field<string>("City")
                        }).ToList();
                    serviceRes.Data = productCategories;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Category Listing";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Category not found";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes NearByDistributors(NearByDistributors request)
        {
            ServiceRes<List<NearByDistributors>> serviceRes = new ServiceRes<List<NearByDistributors>>();
            try {
                SqlParameter [] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter { ParameterName = "@Member_Id", Value = request.UserId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@flag", Value = "ND" };
                DataTable dtNearbyDist = SqlHelper.GetTableFromSP("Usp_Products_Test", sqlParameter);
                if(dtNearbyDist.Rows.Count > 0)
                {
                    List<NearByDistributors> nearByDistributors = dtNearbyDist.AsEnumerable().
                        Select(x => new NearByDistributors {
                            CompanyName=x.Field<string>("Company_Name"),
                            Distance=x.Field<double>("Distance"),
                            UserId =x.Field<int>("Member_Id")
                        }).ToList();
                    serviceRes.Data = nearByDistributors;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Near by distributor";
                }
                else
                {
                    serviceRes.Data = null;
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Distributor not found";
                }
            }
            catch(Exception ex) {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes ProductByCategories(NearByDistributors nearBy)
        {
            ServiceRes<List<ProductListByCategory>> serviceRes = new ServiceRes<List<ProductListByCategory>>();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter { ParameterName = "@Member_Id", Value = nearBy.UserId};
                sqlParameters[1] = new SqlParameter { ParameterName = "@Category_Id", Value = nearBy.CategoryId };
                sqlParameters[2] = new SqlParameter { ParameterName = "@flag", Value = "DPC" };
                DataTable dtProductByCategory = SqlHelper.GetTableFromSP("Usp_Products_Test", sqlParameters);
                if (dtProductByCategory.Rows.Count > 0)
                {
                    List<ProductListByCategory> productDetails = dtProductByCategory.AsEnumerable().
                        Select(x => new ProductListByCategory
                        { 
                            Price = x.Field<decimal>("Price"),
                            Specification = x.Field<string>("Specification"),
                            Quantity = x.Field<int>("Quantity"),
                            Photos_Url = $"http://escandent.com/{x.Field<string>("Photos_Url")}",
                            Name = x.Field<string>("Name"),
                            ProductId=x.Field<int>("Product_Id")
                        }).ToList();
                    serviceRes.Data = productDetails;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Product by Category";
                }
                else
                {
                    serviceRes.Data = null;
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "";
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
