using Entities;
using System.Collections.Generic;

namespace Repository
{
    public interface IProductRepository
    {
        ServiceRes SalesReport();
        ServiceRes AddProduct(ProductDetails objProduct);
        ServiceRes DeleteProduct(int productId);
        ServiceRes UpdateProduct(ProductDetails objProduct);
        ServiceRes GetAllProductDetails();
        ServiceRes GetCategoryMaster();
        ServiceRes GetSubCategoryMaster(int categoryId);
    }
}
