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
        private readonly DB_WareHouseMangerContext _context;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, DB_WareHouseMangerContext context, IConfiguration configuration)
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
        public async Task<JsonResult> GetDetails(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var countReceipt = await statisticsDAO.GetCountShop_Goods_Receipt(startDate, endDate);
            var countIssues = await statisticsDAO.GetCountShop_Goods_Issues(startDate, endDate);
            var revenue = await statisticsDAO.GetCountShop_Goods_Revenue(startDate, endDate);
            var cost = await statisticsDAO.GetCountShop_Goods_Cost(startDate, endDate);
            var realRevenue = await statisticsDAO.GetCountShop_Goods_RealRevenue(startDate, endDate);
            var realCost = await statisticsDAO.GetCountShop_Goods_RealCost(startDate, endDate);

            return Json(new
            {
                data = new
                {
                    countReceipt = countReceipt,
                    countIssues = countIssues,
                    revenue = revenue,
                    cost = cost,
                    realRevenue = realRevenue,
                    realCost = realCost
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_Receipt(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var countReceipt = await statisticsDAO.GetCountShop_Goods_Receipt(startDate, endDate);

            return Json(new
            {
                value = countReceipt
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_Issues(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var countIssues = await statisticsDAO.GetCountShop_Goods_Issues(startDate, endDate);

            return Json(new
            {
                value = countIssues
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_Revenue(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var revenue = await statisticsDAO.GetCountShop_Goods_Revenue(startDate, endDate);

            return Json(new
            {
                value = revenue
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_Cost(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var cost = await statisticsDAO.GetCountShop_Goods_Cost(startDate, endDate);

            return Json(new
            {
                value = cost
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_RealRevenue(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var realRevenue = await statisticsDAO.GetCountShop_Goods_RealRevenue(startDate, endDate);

            return Json(new
            {
                value = realRevenue
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountShop_Goods_RealCost(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var realCost = await statisticsDAO.GetCountShop_Goods_RealCost(startDate, endDate);

            return Json(new
            {
                value = realCost
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRevenueGroupByEmployee(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var data = await statisticsDAO.GetRevenueGroupByEmployee(startDate, endDate);

            return Json(new
            {
                data = data
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetRevenueGroupByCustomer(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var data = await statisticsDAO.GetRevenueGroupByCustomer(startDate, endDate);

            return Json(new
            {
                data = data
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCostGroupBySuplier(DateTime startDate, DateTime endDate)
        {
            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var data = await statisticsDAO.GetCostGroupBySuplier(startDate, endDate);

            return Json(new
            {
                data = data
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
                .Include(t => t.SubCategory)
                .Include(t => t.Unit)
                .Where(t => t.Count <= count && t.Count > 0).ToListAsync();

            return Json(new
            {
                data = list.Select(t => new
                {
                    TemplateID = t.TemplateID,
                    Name = t.Name,
                    //Category = t.SubCategory.SubCategoriName,
                    Unit = t.Unit.Name,
                    Count = t.Count,
                }).AsEnumerable()
            }); ;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetSoldOutOfStock()
        {
            int count = 0;
            var list = await _context.Shop_Goods
                .Include(t => t.SubCategory)
                .Include(t => t.Unit)
                .Where(t => t.Count == count).ToListAsync();

            return Json(new
            {
                data = list.Select(t => new
                {
                    TemplateID = t.TemplateID,
                    Name = t.Name,
                    //Category = t.SubCategory.SubCategoriName,
                    Unit = t.Unit.Name,
                    Count = t.Count,
                }).AsEnumerable()
            }); ;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountRecepitShopGoods(string id, string type, int month, int year)
        {

            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);


            return Json(new
            {
                data = type == "month" ? await statisticsDAO.GetCountRecepitShopGoods(DateTime.Now.Month, DateTime.Now.Year, id) : type == "year" ? await statisticsDAO.GetCountRecepitShopGoods(DateTime.Now.Year, id) : new List<object>()
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountRecepitShopGoodsInYear(string id)
        {

            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);


            return Json(new
            {
                data = await statisticsDAO.GetCountRecepitShopGoods(DateTime.Now.Year, id)
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountRecepitShopGoodsGroupByCustomer(string id, string datatype, int month, int year)
        {

            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var data = new List<CountRecepitShopGoodsGroupByCustomer>();

            if (datatype == "month")
            {
                data = await statisticsDAO.GetCountRecepitShopGoodsGroupByCustomer(month, year, id);
            }
            else
            {
                data = await statisticsDAO.GetCountRecepitShopGoodsGroupByCustomer(year, id);
            }

            return Json(new
            {
                data = data
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetCountRecepitShopGoodsGroupByEmployee(int id, string type, int month, int year)
        {

            Models.DAO.StatisticsDAO statisticsDAO = new Models.DAO.StatisticsDAO(_context);

            var data = new List<object>();

            if (type == "month")
            {
                data = await statisticsDAO.GetCountIssuesShopGoodsGroupByEmployee(month, year, id);
            }
            else
            {
                data = await statisticsDAO.GetCountIssuesShopGoodsGroupByEmployee(year, id);
            }

            return Json(new
            {
                data = data
            });
        }

        public IActionResult Page404()
        {
            return View();
        }
    }
}
