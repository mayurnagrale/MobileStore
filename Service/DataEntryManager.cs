namespace MobileStore.Models
{
    public class DataEntryManager
    {
        public List<SaleRecord> SalesData { get; set; }

        public DataEntryManager()
        {
            SalesData = new List<SaleRecord>();
        }

        public void AddSaleRecord(SaleRecord saleRecord)
        {
            SalesData.Add(saleRecord);
        }

        public void RemoveSaleRecord(SaleRecord saleRecord)
        {
            SalesData.Remove(saleRecord);
        }
    }
}
