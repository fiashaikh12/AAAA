using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class Distributros
    {
        public List<NearByDistributors> Distributors { get; }
    }
    public class NearByDistributors
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        //public string PanNumber { get; set; }
        //public string GSTNumber { get; set; }
        public double Distance { get; set; }
        public string  Kilometer {
            get {
                return string.Format("{0:0.00km}", this.Distance / 100000);
            }
        }
        //public string CompanyPhotoURL { get; set; }
        //public string BusinessName { get; set; }
    }
}
