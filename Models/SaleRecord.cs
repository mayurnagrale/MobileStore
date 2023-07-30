using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileStore.Models
{
    public class SaleRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }
        public string MobileModel { get; set; }
        public string MobileBrand { get; set; }
        public int QuantitySold { get; set; }
        public float SalesPrice { get; set; }
        public float Discount { get; set; }
        public DateTime SaleDate { get; set; }
        public User User { get; set; }
    }
}
