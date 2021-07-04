using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    public class Shop_Goods_StockTakeController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_StockTakeController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        // GET: Shop_Goods_StockTake
        public async Task<IActionResult> Index()
        {
            var dB_WareHouseMangerContext = _context.Shop_Goods_StockTakes.Include(s => s.Employee);
            return View(await dB_WareHouseMangerContext.ToListAsync());
        }

        // GET: Shop_Goods_StockTake/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.StockTakeID == id);
            if (shop_Goods_StockTake == null)
            {
                return NotFound();
            }

            return View(shop_Goods_StockTake);
        }

        // GET: Shop_Goods_StockTake/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID");
            return View();
        }

        // POST: Shop_Goods_StockTake/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockTakeID,DateCreated,Remark,EmployeeID")] Shop_Goods_StockTake shop_Goods_StockTake)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_StockTake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        // GET: Shop_Goods_StockTake/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes.FindAsync(id);
            if (shop_Goods_StockTake == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        // POST: Shop_Goods_StockTake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StockTakeID,DateCreated,Remark,EmployeeID")] Shop_Goods_StockTake shop_Goods_StockTake)
        {
            if (id != shop_Goods_StockTake.StockTakeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_StockTake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_StockTakeExists(shop_Goods_StockTake.StockTakeID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        // GET: Shop_Goods_StockTake/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.StockTakeID == id);
            if (shop_Goods_StockTake == null)
            {
                return NotFound();
            }

            return View(shop_Goods_StockTake);
        }

        // POST: Shop_Goods_StockTake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes.FindAsync(id);
            _context.Shop_Goods_StockTakes.Remove(shop_Goods_StockTake);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_StockTakeExists(int id)
        {
            return _context.Shop_Goods_StockTakes.Any(e => e.StockTakeID == id);
        }
    }
}
