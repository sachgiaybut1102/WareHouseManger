using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Models.EF.DB_WareHouseMangerContext _context;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, Models.EF.DB_WareHouseMangerContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetDetails(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var countReceipt = statisticsDAO.GetCountShop_Goods_Receipt(startDate, endDate);
            var countIssues = statisticsDAO.GetCountShop_Goods_Issues(startDate, endDate);
            var revenue = statisticsDAO.GetCountShop_Goods_Revenue(startDate, endDate);
            var cost = statisticsDAO.GetCountShop_Goods_Cost(startDate, endDate);

            return Json(new
            {
                data = new
                {
                    countReceipt = countReceipt,
                    countIssues = countIssues,
                    revenue = revenue,
                    cost = cost
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetChart(string type, int month, int year)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            List<StatisticsInfo> sumReceipts = new List<StatisticsInfo>();
            List<StatisticsInfo> sumIssues = new List<StatisticsInfo>();

            if (type == "month")
            {
                sumReceipts = await statisticsDAO.GetCountShop_Goods_ReceiptByMonth(month, year);
                sumIssues = await statisticsDAO.GetCountShop_Goods_IssuesByMonth(month, year);
            }
            else if (type == "year")
            {
                sumReceipts = await statisticsDAO.GetCountShop_Goods_ReceiptByYear(year);
                sumIssues = await statisticsDAO.GetCountShop_Goods_IssuesByYear(year);
            }

            return Json(new
            {
                data = new
                {
                    sumReceipts = sumReceipts,
                    sumIssues = sumIssues
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRankTemplate(string type, int month, int year)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            List<StatisticsShopGoodsInfo> list = new List<StatisticsShopGoodsInfo>();

            if (type == "month")
            {
                list = await statisticsDAO.GetRankShop_GoodsByMonth(month, year);
            }
            else if (type == "year")
            {
                list = await statisticsDAO.GetRankShop_GoodsByYear(year);
            }

            return Json(new
            {
                data = list
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetOutOfStock()
        {
            int count = int.Parse(_configuration["OutOfStock:Value"]);
            var list = await _context.Shop_Goods
                .Include(t => t.Category)
                .Include(t => t.Unit)
                .Where(t => t.Count <= count).ToListAsync();

            return Json(new
            {
                data = list.Select(t => new
                {
                    TemplateID = t.TemplateID,
                    Name = t.Name,
                    Category = t.Category.Name,
                    Unit = t.Unit.Name,
                    Count = t.Count,
                }).AsEnumerable()
            }); ;
        }

        public IActionResult Page404()
        {
            return View();
        }
    }
}
