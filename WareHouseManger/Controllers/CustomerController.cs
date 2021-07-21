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
    public class CustomerController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public CustomerController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Customer_Index")]
        // GET: Customer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers
                .Include(t => t.CustomerCategory)
                .OrderByDescending(t => t.CustomerCategoryID)
                .ToListAsync());
        }

        [Authorize(Roles = "Customer_Details")]
        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(t => t.CustomerCategory)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Authorize(Roles = "Account_Create")]
        // GET: Customer/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CustomerCategoryID = new SelectList(await _context.Customer_Categories.ToListAsync(), "CustomerCategoryID", "Name");
            return View();
        }

        [Authorize(Roles = "Account_Create")]
        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Name,PhoneNumber,Address,EMail,CustomerCategoryID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CustomerCategoryID = new SelectList(await _context.Customer_Categories.ToListAsync(), "CustomerCategoryID", "Name", customer.CustomerCategoryID);
            return View(customer);
        }

        [Authorize(Roles = "Customer_Edit")]
        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.CustomerCategoryID = new SelectList(await _context.Customer_Categories.ToListAsync(), "CustomerCategoryID", "Name", customer.CustomerCategoryID);

            return View(customer);
        }

        [Authorize(Roles = "Customer_Edit")]
        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Name,PhoneNumber,Address,EMail,CustomerCategoryID")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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

            ViewBag.CustomerCategoryID = new SelectList(await _context.Customer_Categories.ToListAsync(), "CustomerCategoryID", "Name", customer.CustomerCategoryID);
            return View(customer);
        }

        [Authorize(Roles = "Customer_Delete")]
        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(t => t.CustomerCategory)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Authorize(Roles = "Customer_Delete")]
        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers
                .Include(t => t.CustomerCategory)
                .FirstOrDefaultAsync(m => m.CustomerID == id);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
