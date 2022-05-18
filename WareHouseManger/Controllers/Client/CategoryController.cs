using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using WareHouseManger.ViewModels;
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

        public async Task<IActionResult> Index(int? page = 1, int pageSize = 6, string keyword = "", string Name = "", int subCategroyId = 1, int minPrice = 0, int maxPrice = 500000, string sortType = "default")
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            maxPrice = maxPrice >= 0 ? maxPrice : 0;
            minPrice = minPrice < maxPrice ? minPrice : maxPrice;

            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinPrice = minPrice;

            var shopgoods = await _context.Shop_Goods
                .Where(t => (t.Name.Contains(Name) || t.Name.Contains(keyword)) &&
                t.CostPrice >= minPrice && t.CostPrice <= maxPrice &&
                t.SubCategoryID == subCategroyId)
                .ToListAsync();

            var shopGoodsPageList = await shopgoods.ToPagedListAsync(currentPage, pageSize);

            ViewBag.Shop_Goods = (sortType == "default" || sortType == "name") ? await shopgoods.OrderBy(t => t.Name).ToPagedListAsync(currentPage, pageSize) : 
                await shopgoods.OrderBy(t => t.Price).ToPagedListAsync(currentPage, pageSize);
            ViewBag.SubCategoryId = subCategroyId;

            var sortTypes = new SortTypeViewModel[] { new SortTypeViewModel { Name = "Mặc định", Value = "default" }, new SortTypeViewModel { Name = "Theo giá", Value = "price" }, new SortTypeViewModel { Name = "Theo tên", Value = "name" } };

            ViewBag.SortTypes = sortTypes;
            ViewBag.SortType = sortType;

            var categories = await _context.Shop_Goods_Categories
                .Include(t => t.Shop_Goods_SubCategories)
                .Where(t => t.Name.Contains(keyword))
                .OrderBy(t => t.CategoryID)
                .ToListAsync();

            ViewBag.PageSize = pageSize;

            return View(categories);
        }

        public async Task<IActionResult> GetShopGoodsBySubCategoryId(int? page = 1, int pageSize = 15, string keyword = "", int subCategroyId = -1)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            var shopgoods = await _context.Shop_Goods
                .Where(t => t.Name.Contains(keyword) && t.SubCategoryID == subCategroyId)
                .OrderBy(t => t.Name)
                .ToListAsync();

            ViewBag.Shop_Goods = await shopgoods.ToPagedListAsync(currentPage, pageSize);

            var categories = await _context.Shop_Goods_Categories
                .Include(t => t.Shop_Goods_SubCategories)
                .Where(t => t.Name.Contains(keyword))
                .OrderBy(t => t.CategoryID)
                .ToListAsync();

            return View("Index", await categories.ToPagedListAsync(currentPage, pageSize));
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
