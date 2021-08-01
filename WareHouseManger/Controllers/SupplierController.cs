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
    public class SupplierController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public SupplierController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Supplier_Index")]
        // GET: Supplier
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Suppliers
                .Where(t => t.Name.Contains(keyword) && !(bool)t.IsDelete)
                .OrderByDescending(t => t.SupplierID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "Supplier_Details")]
        // GET: Supplier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [Authorize(Roles = "Supplier_Create")]
        // GET: Supplier/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Supplier_Create")]
        // POST: Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,Name,PhoneNumber,Address,EMail")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                supplier.IsDelete = false;
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        [Authorize(Roles = "Supplier_Edit")]
        // GET: Supplier/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [Authorize(Roles = "Supplier_Edit")]
        // POST: Supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,Name,PhoneNumber,Address,EMail")] Supplier supplier)
        {
            if (id != supplier.SupplierID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supplier.IsDelete = false;
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierID))
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
            return View(supplier);
        }

        [Authorize(Roles = "Supplier_Delete")]
        // GET: Supplier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [Authorize(Roles = "Supplier_Delete")]
        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                supplier.IsDelete = true;
                //_context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
