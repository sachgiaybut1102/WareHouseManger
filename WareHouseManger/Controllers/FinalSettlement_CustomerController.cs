using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    public class FinalSettlement_CustomerController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public FinalSettlement_CustomerController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "FinalSettlement_Customer_Index")]
        // GET: FinalSettlement_Customer
        public async Task<IActionResult> Index()
        {
            //var dB_WareHouseMangerContext = _context.FinalSettlement_Customers.Include(f => f.Customer).Include(f => f.GoodsIssues);
            var dB_WareHouseMangerContext = _context.Customers
                .Include(t => t.FinalSettlement_Customers)
                .Include(t => t.Shop_Goods_Issues);

            return View(await dB_WareHouseMangerContext.ToListAsync());
        }

        [Authorize(Roles = "FinalSettlement_Customer_Details")]
        // GET: FinalSettlement_Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalSettlement_Customer = await _context.FinalSettlement_Customers
                .Include(f => f.Customer)
                .Include(f => f.GoodsIssues)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (finalSettlement_Customer == null)
            {
                return NotFound();
            }

            return View(finalSettlement_Customer);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Create")]
        // GET: FinalSettlement_Customer/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID");
            ViewData["GoodsIssuesID"] = new SelectList(_context.Shop_Goods_Issues, "GoodsIssueID", "GoodsIssueID");
            return View();
        }

        [Authorize(Roles = "FinalSettlement_Customer_Create")]
        // POST: FinalSettlement_Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CustomerID,GoodsIssuesID,DateCreated,Payment,Remainder")] FinalSettlement_Customer finalSettlement_Customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finalSettlement_Customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", finalSettlement_Customer.CustomerID);
            ViewData["GoodsIssuesID"] = new SelectList(_context.Shop_Goods_Issues, "GoodsIssueID", "GoodsIssueID", finalSettlement_Customer.GoodsIssuesID);
            return View(finalSettlement_Customer);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Edit")]
        // GET: FinalSettlement_Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalSettlement_Customer = await _context.FinalSettlement_Customers.FindAsync(id);
            if (finalSettlement_Customer == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", finalSettlement_Customer.CustomerID);
            ViewData["GoodsIssuesID"] = new SelectList(_context.Shop_Goods_Issues, "GoodsIssueID", "GoodsIssueID", finalSettlement_Customer.GoodsIssuesID);
            return View(finalSettlement_Customer);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Edit")]
        // POST: FinalSettlement_Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CustomerID,GoodsIssuesID,DateCreated,Payment,Remainder")] FinalSettlement_Customer finalSettlement_Customer)
        {
            if (id != finalSettlement_Customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finalSettlement_Customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinalSettlement_CustomerExists(finalSettlement_Customer.ID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", finalSettlement_Customer.CustomerID);
            ViewData["GoodsIssuesID"] = new SelectList(_context.Shop_Goods_Issues, "GoodsIssueID", "GoodsIssueID", finalSettlement_Customer.GoodsIssuesID);
            return View(finalSettlement_Customer);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Delete")]
        // GET: FinalSettlement_Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalSettlement_Customer = await _context.FinalSettlement_Customers
                .Include(f => f.Customer)
                .Include(f => f.GoodsIssues)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (finalSettlement_Customer == null)
            {
                return NotFound();
            }

            return View(finalSettlement_Customer);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Delete")]
        // POST: FinalSettlement_Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var finalSettlement_Customer = await _context.FinalSettlement_Customers.FindAsync(id);
            _context.FinalSettlement_Customers.Remove(finalSettlement_Customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinalSettlement_CustomerExists(int id)
        {
            return _context.FinalSettlement_Customers.Any(e => e.ID == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetByCustomerID(int customerID)
        {
            List<FinalSettlement_Customer> finalSettlement_Customers = await _context.FinalSettlement_Customers
                .Where(t => t.CustomerID == customerID)
                .OrderByDescending(t => t.DateCreated)
                .ToListAsync();

            return Json(new
            {
                data = finalSettlement_Customers
            });
        }
    }
}
