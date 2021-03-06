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
    public class Shop_Goods_UnitController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_UnitController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Unit_Index")]
        // GET: Shop_Goods_Unit
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_Units
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.UnitID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }

        [Authorize(Roles = "Shop_Goods_Unit_Details")]
        // GET: Shop_Goods_Unit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Unit = await _context.Shop_Goods_Units
                .FirstOrDefaultAsync(m => m.UnitID == id);
            if (shop_Goods_Unit == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Unit);
        }

        [Authorize(Roles = "Shop_Goods_Unit_Create")]
        // GET: Shop_Goods_Unit/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Shop_Goods_Unit_Create")]
        // POST: Shop_Goods_Unit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitID,Name")] Shop_Goods_Unit shop_Goods_Unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_Unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop_Goods_Unit);
        }

        [Authorize(Roles = "Shop_Goods_Unit_Edit")]
        // GET: Shop_Goods_Unit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Unit = await _context.Shop_Goods_Units.FindAsync(id);
            if (shop_Goods_Unit == null)
            {
                return NotFound();
            }
            return View(shop_Goods_Unit);
        }

        [Authorize(Roles = "Shop_Goods_Unit_Edit")]
        // POST: Shop_Goods_Unit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnitID,Name")] Shop_Goods_Unit shop_Goods_Unit)
        {
            if (id != shop_Goods_Unit.UnitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_Unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_UnitExists(shop_Goods_Unit.UnitID))
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
            return View(shop_Goods_Unit);
        }

        [Authorize(Roles = "Shop_Goods_Unit_Delete")]
        // GET: Shop_Goods_Unit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {          
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Unit = await _context.Shop_Goods_Units
                .FirstOrDefaultAsync(m => m.UnitID == id);
            if (shop_Goods_Unit == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Unit);
        }

        [Authorize(Roles = "Shop_Goods_Unit_Delete")]
        // POST: Shop_Goods_Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var shop_Goods_Unit = await _context.Shop_Goods_Units.FindAsync(id);
                _context.Shop_Goods_Units.Remove(shop_Goods_Unit);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_UnitExists(int id)
        {
            return _context.Shop_Goods_Units.Any(e => e.UnitID == id);
        }
    }
}
