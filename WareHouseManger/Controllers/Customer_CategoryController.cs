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
    public class Customer_CategoryController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Customer_CategoryController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Customer_Category_Index")]
        // GET: Customer_Category
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Customer_Categories
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.CustomerCategoryID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "Customer_Category_Details")]
        // GET: Customer_Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Customer_Categories
                .FirstOrDefaultAsync(m => m.CustomerCategoryID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        [Authorize(Roles = "Customer_Category_Create")]
        // GET: Customer_Category/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Customer_Category_Create")]
        // POST: Customer_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerCategoryID,Name,Remark")] Customer_Category producer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        [Authorize(Roles = "Customer_Category_Edit")]
        // GET: Customer_Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Customer_Categories.FindAsync(id);
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }

        [Authorize(Roles = "Customer_Category_Edit")]
        // POST: Customer_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerCategoryID,Name,Remark")] Customer_Category producer)
        {
            if (id != producer.CustomerCategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Customer_CategoryExists(producer.CustomerCategoryID))
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
            return View(producer);
        }

        [Authorize(Roles = "Customer_Category_Delete")]
        // GET: Customer_Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Customer_Categories
                .FirstOrDefaultAsync(m => m.CustomerCategoryID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        [Authorize(Roles = "Customer_Category_Delete")]
        // POST: Customer_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var producer = await _context.Customer_Categories.FindAsync(id);
                _context.Customer_Categories.Remove(producer);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Customer_CategoryExists(int id)
        {
            return _context.Customer_Categories.Any(e => e.CustomerCategoryID == id);
        }
    }
}
