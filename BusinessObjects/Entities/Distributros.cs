using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Distributor_User
    {
        public int UserId { get; set; }
        public DateTime ? FilterDate { get; set; }
    }
    public class Distributors
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
    public class ProductListByCategory:Distributor_User
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Specification { get; set; }
        public int Quantity { get; set; }
        public string Photos_Url { get; set; }
        public string Name { get; set; }
    }
    public class NearByDistributors
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public int PinCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double Distance { get; set; }
        public string  Kilometer {
            get {
                return string.Format("{0:0.00km}", this.Distance / 100000);
            }
        }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Building_Name { get; set; }
        public string Locality { get; set; }
    }
    public class DistributorSalesReport
    {
        public DateTime SalesDate { get; set; }
        public int SalesCount { get; set; }
    }
    public class OrderStatus
    {
        public int OrderCount { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
    public class RecentReport
    {
        public string EmailAddress{ get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int UserId { get; set; }
        public DateTime ? OrderDate { get; set; }
        public int ProductId { get; set; }
        public string MechantName { get; set; }
        public string Locality { get; set; }
        public string  PinCode { get; set; }
        public int OrderQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string  ProductName { get; set; }
        public decimal Price { get; set; }
    }
    public class ConfirmOrder
    {
        public List<RecentReport> Request { get; set; }
    }
}
