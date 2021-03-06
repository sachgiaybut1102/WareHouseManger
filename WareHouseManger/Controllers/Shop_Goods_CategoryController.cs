using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    public class Shop_Goods_CategoryController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_CategoryController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Category_Index")]
        // GET: Shop_Goods_Category
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_Categories
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.CategoryID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "Shop_Goods_Category_Details")]
        // GET: Shop_Goods_Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category = await _context.Shop_Goods_Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (shop_Goods_Category == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Category);
        }

        [Authorize(Roles = "Shop_Goods_Category_Create")]
        // GET: Shop_Goods_Category/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Shop_Goods_Category_Create")]
        // POST: Shop_Goods_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Name,SortName")] Shop_Goods_Category shop_Goods_Category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop_Goods_Category);
        }

        [Authorize(Roles = "Shop_Goods_Category_Edit")]
        // GET: Shop_Goods_Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category = await _context.Shop_Goods_Categories.FindAsync(id);
            if (shop_Goods_Category == null)
            {
                return NotFound();
            }
            return View(shop_Goods_Category);
        }

        [Authorize(Roles = "Shop_Goods_Category_Edit")]
        // POST: Shop_Goods_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,Name,SortName")] Shop_Goods_Category shop_Goods_Category)
        {
            if (id != shop_Goods_Category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_Category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_CategoryExists(shop_Goods_Category.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shop_Goods_Category);
        }

        [Authorize(Roles = "Shop_Goods_Category_Delete")]
        // GET: Shop_Goods_Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category = await _context.Shop_Goods_Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (shop_Goods_Category == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Category);
        }

        [Authorize(Roles = "Shop_Goods_Category_Delete")]
        // POST: Shop_Goods_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var shop_Goods_Category = await _context.Shop_Goods_Categories.FindAsync(id);
                _context.Shop_Goods_Categories.Remove(shop_Goods_Category);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_CategoryExists(int id)
        {
            return _context.Shop_Goods_Categories.Any(e => e.CategoryID == id);
        }
    }
}
