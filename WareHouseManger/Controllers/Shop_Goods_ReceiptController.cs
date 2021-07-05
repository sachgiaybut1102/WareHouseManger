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
    public class Shop_Goods_ReceiptController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_ReceiptController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        // GET: Shop_Goods_Receipt
        public async Task<IActionResult> Index()
        {
            var dB_WareHouseMangerContext = _context.Shop_Goods_Receipts.Include(s => s.Employee).Include(s => s.Supplier);
            return View(await dB_WareHouseMangerContext.ToListAsync());
        }

        // GET: Shop_Goods_Receipt/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Receipt = await _context.Shop_Goods_Receipts
                .Include(s => s.Employee)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.GoodsReceiptID == id);
            if (shop_Goods_Receipt == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Receipt);
        }

        // GET: Shop_Goods_Receipt/Create
        public IActionResult Create()
        {
            var model = new Shop_Goods_Receipt()
            {
                DateCreated = DateTime.Now
            };

            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");

            return View(model);
        }

        // POST: Shop_Goods_Receipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsReceiptID,DateCreated,SupplierID,Remark,Total,EmployeeID")] Shop_Goods_Receipt shop_Goods_Receipt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop_Goods_Receipt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Receipt.EmployeeID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", shop_Goods_Receipt.SupplierID);
            return View(shop_Goods_Receipt);
        }

        // GET: Shop_Goods_Receipt/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Receipt = await _context.Shop_Goods_Receipts.FindAsync(id);
            if (shop_Goods_Receipt == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Receipt.EmployeeID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", shop_Goods_Receipt.SupplierID);
            return View(shop_Goods_Receipt);
        }

        // POST: Shop_Goods_Receipt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GoodsReceiptID,DateCreated,SupplierID,Remark,Total,EmployeeID")] Shop_Goods_Receipt shop_Goods_Receipt)
        {
            if (id != shop_Goods_Receipt.GoodsReceiptID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Goods_Receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_Goods_ReceiptExists(shop_Goods_Receipt.GoodsReceiptID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Receipt.EmployeeID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", shop_Goods_Receipt.SupplierID);
            return View(shop_Goods_Receipt);
        }

        // GET: Shop_Goods_Receipt/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Receipt = await _context.Shop_Goods_Receipts
                .Include(s => s.Employee)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.GoodsReceiptID == id);
            if (shop_Goods_Receipt == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Receipt);
        }

        // POST: Shop_Goods_Receipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shop_Goods_Receipt = await _context.Shop_Goods_Receipts.FindAsync(id);
            _context.Shop_Goods_Receipts.Remove(shop_Goods_Receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_ReceiptExists(string id)
        {
            return _context.Shop_Goods_Receipts.Any(e => e.GoodsReceiptID == id);
        }
    }
}
