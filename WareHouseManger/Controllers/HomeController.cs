using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models;

namespace WareHouseManger.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Models.EF.DB_WareHouseMangerContext _context;
        public HomeController(ILogger<HomeController> logger, Models.EF.DB_WareHouseMangerContext context)
        {
            _logger = logger;
            _context = context;
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
    }
}
