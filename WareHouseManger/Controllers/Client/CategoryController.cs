using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers.Client
{
    public class CategoryController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public CategoryController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page = 1, int pageSize = 15, string keyword = "", string Name = "")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;
            ViewBag.Shop_Goods = _context.Shop_Goods
                .Where(t => t.Name.Equals(Name))
                .OrderBy(t => t.Name)
                .ToList()
                .ToPagedListAsync(currentPage, pageSize);
            return View(await _context.Shop_Goods_Categories
                .Include(t => t.Shop_Goods_SubCategories)
                .Where(t => t.Name.Contains(keyword))
                .OrderBy(t => t.CategoryID)
                .ToList()
                .ToPagedListAsync(currentPage, pageSize));
        }

        public async Task<IActionResult> GetCategoryChildren(int? page = 1, int pageSize = 15, string keyword = "")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;
            ViewBag.Shop_Goods = _context.Shop_Goods
                .Where(t => t.Name.ToString().Contains(keyword))
                .OrderByDescending(t => t.Name)
                .ToList()
                .ToPagedListAsync(currentPage, 10);
            return PartialView(await _context.Shop_Goods
                .Where(t => t.Name.ToString().Contains(keyword))
                .OrderByDescending(t => t.Name)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }
    }
}
