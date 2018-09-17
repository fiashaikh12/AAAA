using System;
using System.Collections.Generic;
using Repository;
using Entities;
using DataAccessLayer;
using System.Data;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public ServiceRes AddProduct(ProductDetails objProduct)
        {
            throw new NotImplementedException();
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
