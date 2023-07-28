namespace MobileStore.Models
{
    public class SaleRecord
    {
        public int SaleId { get; set; }
        public string MobileModel { get; set; }
        public string MobileBrand { get; set; }
        public int QuantitySold { get; set; }
        public float SalesPrice { get; set; }
        public float Discount { get; set; }
        public User User { get; set; }
    }
}
