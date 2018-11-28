using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Distributros
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        //public List<NearByDistributors> Distributors { get; }
    }
    public class ProductListByCategory
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
}
