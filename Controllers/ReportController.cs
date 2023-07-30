using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Data;
using MobileStore.Models;
using Rotativa;

namespace MobileStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly MobileStoreContext _context;
        List<SaleRecord> salesRecords;
        public ReportController(MobileStoreContext context)
        {
            _context = context;
            salesRecords = _context.SaleRecords.ToList();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MSReport(SaleRecord ?saleRecord, int? selectedMonth)
        {
            // Get distinct months from the sales records for the dropdown
            ViewBag.Months = _context.SaleRecords.Select(s => s.SaleDate.Month).Distinct().ToList();

            // Set default value for the dropdown if not provided
            int selectedMonthValue = selectedMonth ?? DateTime.Now.Month;
            ViewBag.SelectedMonth = selectedMonthValue;

            // Get distinct brands from the sales records for the dropdown
            ViewBag.Brands = _context.SaleRecords.Select(s => s.MobileBrand).Distinct().ToList();

            // Set default values for the dropdowns if not provided
            //ViewBag.SelectedBrand = selectedBrand ?? 0;

            // Retrieve sales records for the selected month
            var salesRecords = _context.SaleRecords
        .Where(s => s.SaleDate.Month == selectedMonthValue)
        .ToList();

            return View(salesRecords);
        }

        [HttpPost]
        public ActionResult SelectMonth(int? selectedMonth)
        {
            return RedirectToAction("MSReport", new { selectedMonth });
        }

        //// GET: Report/GeneratePDF
        //public ActionResult GeneratePDF(int? selectedMonth)
        //{
        //    // Retrieve sales records for the selected month
        //    var salesRecords = _context.SaleRecords
        //        .Where(s => s.SaleDate.Month == selectedMonth)
        //        .ToList();

        //    // Generate PDF using Rotativa
        //    var pdfResult = new ViewAsPdf("PDFView", salesRecords)
        //    {
        //        FileName = "SalesReport.pdf"
        //    };

        //    // Return the PDF result
        //    return pdfResult;
        //}

        // GET: Report/GeneratePDF
        public ActionResult GeneratePDF(int? selectedMonth)
        {
            // Retrieve sales records for the selected month
            var salesRecords = _context.SaleRecords
                .Where(s => s.SaleDate.Month == selectedMonth)
                .ToList();

            // Generate PDF using iTextSharp
            byte[] pdfBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                // Create the PDF document
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Add content to the PDF
                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.AddCell("Sale ID");
                table.AddCell("Mobile Brand");
                table.AddCell("Mobile Model");
                table.AddCell("Quantity Sold");
                table.AddCell("Sales Price");
                table.AddCell("Discount");
                table.AddCell("SaleDate");

                foreach (var record in salesRecords)
                {
                    table.AddCell(record.SaleId.ToString());
                    table.AddCell(record.MobileBrand);
                    table.AddCell(record.MobileModel);
                    table.AddCell(record.QuantitySold.ToString());
                    table.AddCell(record.SalesPrice.ToString());
                    table.AddCell(record.Discount.ToString());
                    table.AddCell(record.SaleDate.ToString());
                }

                document.Add(table);
                document.Close();

                // Get the bytes of the PDF
                pdfBytes = ms.ToArray();
            }

            // Return the PDF result
            return File(pdfBytes, "application/pdf", "SalesReport.pdf");
        }


        public IActionResult BSReport()
        {
            return View(salesRecords);
        }

        public IActionResult PLReport()
        {
            return View();
        }
    }
}
