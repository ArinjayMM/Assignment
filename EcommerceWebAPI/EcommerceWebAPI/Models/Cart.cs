using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace EcommerceWebAPI.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal TotalPrice { get; set; }

        [ForeignKey("ProductID")]
        public Products? products { get; set; }

    }
}
