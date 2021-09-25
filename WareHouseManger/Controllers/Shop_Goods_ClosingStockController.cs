using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    [Authorize]
    public class Shop_Goods_ClosingStockController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;
        public Shop_Goods_ClosingStockController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Create()
        {
            bool isSuccess = true;
            string msg = "Chốt sổ thành công!";

            Shop_Goods_ClosingStock shop_Goods_ClosingStock = await _context.Shop_Goods_ClosingStocks.OrderBy(t => t.ClosingStockID).LastOrDefaultAsync();

            if (shop_Goods_ClosingStock == null)
            {
                await InitShop_Goods_ClosingStock();
            }
            else
            {
                DateTime lastDate = new DateTime(
                    shop_Goods_ClosingStock.DateClosing.Value.Year,
                    shop_Goods_ClosingStock.DateClosing.Value.Month,
                    shop_Goods_ClosingStock.DateClosing.Value.Day);

                DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (lastDate < newDate)
                {
                    await InitShop_Goods_ClosingStock();
                }
                else
                {
                    isSuccess = false;
                    msg = "Sổ kho đã được chốt, vui lòng quay lại vào ngày mai!";
                }
            }
            return Json(new { isSuccess = isSuccess, msg = msg });
        }

        private async Task InitShop_Goods_ClosingStock()
        {
            Shop_Goods_ClosingStock shop_Goods_ClosingStock = new Shop_Goods_ClosingStock()
            {
                ClosingStockID = string.Format("CS{0}{1}{2}",
                DateTime.Now.Year,
                DateTime.Now.Month >= 10 ? DateTime.Now.Month : ("0" + DateTime.Now.Month),
                DateTime.Now.Day >= 10 ? DateTime.Now.Day : ("0" + DateTime.Now.Day)),
                DateClosing = DateTime.Now,
                Name = "Kì 1 - " + DateTime.Now.Year,
            };

            

            //Add Details
            var templates = await _context.Shop_Goods.ToListAsync();

            List<Shop_Goods_ClosingStock_Detail> shop_Goods_ClosingStock_Details = new List<Shop_Goods_ClosingStock_Detail>();

            foreach (var template in templates)
            {
                var shop_Goods_ClosingStock_Detail = new Shop_Goods_ClosingStock_Detail()
                {
                    ClosingStockID = shop_Goods_ClosingStock.ClosingStockID,
                    TemplateID = template.TemplateID,
                    Count = template.Count
                };

                shop_Goods_ClosingStock_Details.Add(shop_Goods_ClosingStock_Detail);
            }
            shop_Goods_ClosingStock.Shop_Goods_ClosingStock_Details = shop_Goods_ClosingStock_Details;
            await _context.Shop_Goods_ClosingStocks.AddAsync(shop_Goods_ClosingStock);
            await _context.SaveChangesAsync();
        }
    }
}
