using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers.Client
{
    public class ProductController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public ProductController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Index")]
        // GET: Shop_Goods
        public async Task<IActionResult> Index(int? page = 1, int pageSize = 15, string keyword = "")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods
                .Include(s => s.SubCategory)
                .Include(s => s.Producer)
                .Include(s => s.Unit)
                .Include(s => s.Shop_Goods_Issues_Details)
                .ThenInclude(s => s.GoodsIssue)
                .Include(s => s.Shop_Goods_Receipt_Details)
                .ThenInclude(s => s.GoodsReceipt)
                .Where(t => t.TemplateID.Contains(keyword) || t.Name.Contains(keyword))
                .OrderByDescending(t => t.TemplateID)
                .ToList()
                .ToPagedListAsync(currentPage, pageSize));
        }
    }
}
