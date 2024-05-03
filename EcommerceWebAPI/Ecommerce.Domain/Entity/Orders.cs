

using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebAPI.Models
{
    public class Orders
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string OrderNo { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [ForeignKey("ProductID")]
        public Products? products { get; set; }
    }
}

