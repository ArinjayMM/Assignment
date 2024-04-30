namespace EcommerceWebAPI.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Type { get; set; } = "user";
        public int Status { get; set; } = 0;
    }
}
