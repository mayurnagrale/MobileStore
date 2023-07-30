namespace MobileStore.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }
        public List<SaleRecord> SaleRecords { get; set; }
        public User()
        {
            Roles = new List<Role>();
        }
    }
}
