using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    public class Shop_Goods_Category_ParentController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_Category_ParentController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Index")]
        // GET: Shop_Goods_Category_Parent
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_Category_Parents
                .Include(t=>t.Shop_Goods_Category_Children)
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.CategoryParentID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Details")]
        // GET: Shop_Goods_Category_Parent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category_Parents = await _context.Shop_Goods_Category_Parents
                .FirstOrDefaultAsync(m => m.CategoryParentID == id);
            if (shop_Goods_Category_Parents == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Category_Parents);
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Create")]
        // GET: Shop_Goods_Category/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Create")]
        // POST: Shop_Goods_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryChildID,Name,SortName")] Shop_Goods_Category_Parent shop_Goods_Category_Parent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_Category_Parent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop_Goods_Category_Parent);
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Edit")]
        // GET: Shop_Goods_Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category_Parent = await _context.Shop_Goods_Category_Parents.FindAsync(id);
            if (shop_Goods_Category_Parent == null)
            {
                return NotFound();
            }
            return View(shop_Goods_Category_Parent);
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Edit")]
        // POST: Shop_Goods_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryChildID,Name,SortName")] Shop_Goods_Category_Parent shop_Goods_Category_Parent)
        {
            if (id != shop_Goods_Category_Parent.CategoryParentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_Category_Parent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_CategoryExists(shop_Goods_Category_Parent.CategoryParentID))
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
            return View(shop_Goods_Category_Parent);
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Delete")]
        // GET: Shop_Goods_Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Category_Parent = await _context.Shop_Goods_Category_Parents
                .FirstOrDefaultAsync(m => m.CategoryParentID == id);
            if (shop_Goods_Category_Parent == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Category_Parent);
        }

        [Authorize(Roles = "Shop_Goods_Category_Parent_Delete")]
        // POST: Shop_Goods_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var shop_Goods_Category_Parent = await _context.Shop_Goods_Category_Parents.FindAsync(id);
                _context.Shop_Goods_Category_Parents.Remove(shop_Goods_Category_Parent);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_CategoryExists(int id)
        {
            return _context.Shop_Goods_Category_Parents.Any(e => e.CategoryParentID == id);
        }
    }
}
