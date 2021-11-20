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

        public async Task<IActionResult> Index(int? page = 1, int pageSize = 15, string keyword = "")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;
            return View(await _context.Shop_Goods_Category_Parents
                .Include(t => t.Shop_Goods_Category_Children)
                .Where(t => t.Name.Contains(keyword))
                .OrderBy(t => t.CategoryParentID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }

        public async Task<IActionResult> GetCategoryChildren(int? page = 1, int pageSize = 15, string keyword = "")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;
            var a = _context.Shop_Goods_Category_Children
                .Include(t => t.CategoryParent)
                .Where(t => t.CategoryParent.Name.Contains(keyword))
                .OrderBy(t => t.CategoryChildID)
                .ToList()
                .ToPagedListAsync(currentPage, 10);
            return View(await _context.Shop_Goods_Category_Children
                .Include(t => t.CategoryParent)
                .Where(t => t.CategoryParentID.ToString().Contains(keyword))
                .OrderByDescending(t => t.Name)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }
    } 
}
