using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly DB_WareHouseMangerContext _context;
        private readonly IConfiguration _configuration;

        public ReportController(IWebHostEnvironment webHostEnvironment, DB_WareHouseMangerContext context, IConfiguration configuration)
        {
            _webHostEnviroment = webHostEnvironment;
            _context = context;
            _configuration = configuration;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Shop_Goods_Issues(string id)
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{this._webHostEnviroment.WebRootPath}\\Report\\rptShop_Goods_Issues.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("rp0", "Hello");
            //parameters.Add("paDateTime", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));

            var model = await _context.Shop_Goods_Issues
                .Where(t => t.GoodsIssueID == id)
                .Include(t => t.Customer)
                .Include(t => t.Employee)
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Shop_Goods_Issues_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return RedirectToAction("Page404", "Home");
            }

            parameters.Add("paCompanyName", _configuration["CompanyInfomation:Name"]);
            parameters.Add("paCompanyAddress", _configuration["CompanyInfomation:Address"]);
            parameters.Add("paCompanyPhoneNumber", _configuration["CompanyInfomation:PhoneNumber"]);
            parameters.Add("paCompanyWebsite", _configuration["CompanyInfomation:Website"]);
            parameters.Add("paLogo", "file:\\");

            parameters.Add("paDateCreated", string.Format("Ngày {0} tháng {1} năm {2}",
                model.DateCreated.Value.Day,
                model.DateCreated.Value.Month,
                model.DateCreated.Value.Year));
            parameters.Add("paDateReport", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            parameters.Add("paCustomerName", model.Customer.Name);
            parameters.Add("paCustomerPhoneNumber", model.Customer.PhoneNumber);
            parameters.Add("paCustomerAddress", model.Customer.Address);
            parameters.Add("paRemark", model.Remark == null ? " " : model.Remark);
            parameters.Add("paEmployeeName", model.Employee.Name);
            parameters.Add("paGoodsIssuesID", model.GoodsIssueID);
            parameters.Add("paCustomerID", model.CustomerID.ToString());
            parameters.Add("paEmployeeID", model.EmployeeID.ToString());
            parameters.Add("paTotal", string.Format("{0:N}", model.Shop_Goods_Issues_Details.Select(t => (decimal)t.Count * t.UnitPrice).Sum()).Replace(".00", ""));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", model.Shop_Goods_Issues_Details
                .Select(t => new
                {
                    GoodsIssueID = t.GoodsIssueID,
                    TemplateID = t.TemplateID,
                    Name = t.Template.Name,
                    CategoryName = t.Template.Category.Name,
                    UnitName = t.Template.Unit.Name,
                    Count = t.Count,
                    UnitPrice = t.UnitPrice,
                }));

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

            return File(result.MainStream, "application/pdf");
        }

        public async Task<IActionResult> Shop_Goods_Receipt(string id)
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{this._webHostEnviroment.WebRootPath}\\Report\\rptShop_Goods_Receipt.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("rp0", "Hello");
            //parameters.Add("paDateTime", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));

            var model = await _context.Shop_Goods_Receipts
                .Where(t => t.GoodsReceiptID == id)
                .Include(t => t.Supplier)
                .Include(t => t.Employee)
                .Include(t => t.Shop_Goods_Receipt_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Unit)
                .Include(t => t.Shop_Goods_Receipt_Details)
                .ThenInclude(t => t.Template)
                .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return RedirectToAction("Page404", "Home");
            }

            parameters.Add("paCompanyName", _configuration["CompanyInfomation:Name"]);
            parameters.Add("paCompanyAddress", _configuration["CompanyInfomation:Address"]);
            parameters.Add("paCompanyPhoneNumber", _configuration["CompanyInfomation:PhoneNumber"]);
            parameters.Add("paCompanyWebsite", _configuration["CompanyInfomation:Website"]);
            parameters.Add("paLogo", "file:\\");

            parameters.Add("paDateCreated", string.Format("Ngày {0} tháng {1} năm {2}",
                model.DateCreated.Value.Day,
                model.DateCreated.Value.Month,
                model.DateCreated.Value.Year));
            parameters.Add("paDateReport", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            parameters.Add("paSupplierName", model.Supplier.Name);
            parameters.Add("paSupplierPhoneNumber", model.Supplier.PhoneNumber);
            parameters.Add("paSupplierAddress", model.Supplier.Address);
            parameters.Add("paRemark", model.Remark == null ? " " : model.Remark);
            parameters.Add("paEmployeeName", model.Employee.Name);
            parameters.Add("paGoodsReceiptID", model.GoodsReceiptID);
            parameters.Add("paSupplierID", model.Supplier.SupplierID.ToString());
            parameters.Add("paEmployeeID", model.EmployeeID.ToString());
            parameters.Add("paTotal", string.Format("{0:N}", model.Shop_Goods_Receipt_Details.Select(t => (decimal)t.Count * t.UnitPrice).Sum()).Replace(".00", ""));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", model.Shop_Goods_Receipt_Details
                .Select(t => new
                {
                    GoodsIssueID = t.GoodsReceiptID,
                    TemplateID = t.TemplateID,
                    Name = t.Template.Name,
                    CategoryName = t.Template.Category.Name,
                    UnitName = t.Template.Unit.Name,
                    Count = t.Count,
                    UnitPrice = t.UnitPrice,
                }));

            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

            return File(result.MainStream, "application/pdf");
        }
    }
}
