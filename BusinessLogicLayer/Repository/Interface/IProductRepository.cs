using Entities;
using System.Collections.Generic;

namespace Repository
{
    public interface IProductRepository
    {
        ServiceRes OrderSalesReport(Distributor_User distributor_User);
        ServiceRes DeliveredOrderReport();
        ServiceRes AddProduct(ProductDetails objProduct);
        ServiceRes DeleteProduct(int productId);
        ServiceRes UpdateProduct(ProductDetails objProduct);
        ServiceRes GetAllProduct();
        ServiceRes GetProductDetailById();
        ServiceRes GetCategoryMaster();
        ServiceRes GetSubCategoryMaster(int categoryId);
        ServiceRes GetAllRecentOrders();
        ServiceRes RecentOrderDetail(Distributor_User distributor_User);
    }
}
