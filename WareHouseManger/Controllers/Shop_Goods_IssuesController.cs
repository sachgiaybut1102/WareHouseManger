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
    public class Shop_Goods_IssuesController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_IssuesController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        // GET: Shop_Goods_Issues
        public async Task<IActionResult> Index()
        {
            var dB_WareHouseMangerContext = _context.Shop_Goods_Issues.Include(s => s.Customer);
            return View(await dB_WareHouseMangerContext.ToListAsync());
        }

        // GET: Shop_Goods_Issues/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Issue = await _context.Shop_Goods_Issues
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.GoodsIssueID == id);
            if (shop_Goods_Issue == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Issue);
        }

        // GET: Shop_Goods_Issues/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID");
            return View();
        }

        // POST: Shop_Goods_Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsIssueID,DateCreated,CustomerID,Remark,Total,EmployeeID")] Shop_Goods_Issue shop_Goods_Issue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_Issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        // GET: Shop_Goods_Issues/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Issue = await _context.Shop_Goods_Issues.FindAsync(id);
            if (shop_Goods_Issue == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        // POST: Shop_Goods_Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GoodsIssueID,DateCreated,CustomerID,Remark,Total,EmployeeID")] Shop_Goods_Issue shop_Goods_Issue)
        {
            if (id != shop_Goods_Issue.GoodsIssueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_Issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_IssueExists(shop_Goods_Issue.GoodsIssueID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        // GET: Shop_Goods_Issues/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Issue = await _context.Shop_Goods_Issues
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.GoodsIssueID == id);
            if (shop_Goods_Issue == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Issue);
        }

        // POST: Shop_Goods_Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shop_Goods_Issue = await _context.Shop_Goods_Issues.FindAsync(id);
            _context.Shop_Goods_Issues.Remove(shop_Goods_Issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_IssueExists(string id)
        {
            return _context.Shop_Goods_Issues.Any(e => e.GoodsIssueID == id);
        }
    }
}
