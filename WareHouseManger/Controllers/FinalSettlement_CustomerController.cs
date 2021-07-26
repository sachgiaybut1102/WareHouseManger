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
    public class FinalSettlement_CustomerController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public FinalSettlement_CustomerController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "FinalSettlement_Customer_Index")]
        // GET: FinalSettlement_Customer
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Customers
                .Include(t => t.FinalSettlement_Customers)
                .Include(t => t.Shop_Goods_Issues)
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.CustomerID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "FinalSettlement_Customer_Details")]
        // GET: FinalSettlement_Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var finalSettlement_Customer = await _context.FinalSettlement_Customers
            //    .Include(f => f.Customer)
            //    .Include(f => f.GoodsIssues)
            //    .FirstOrDefaultAsync(m => m.ID == id);

            List<Shop_Goods_Issue> shop_Goods_Issue = await _context.Shop_Goods_Issues
                .Where(t => t.CustomerID == id)
                .Include(t => t.FinalSettlement_Customers)
                .Include(t => t.Employee)
                .OrderByDescending(t => t.GoodsIssueID)
                .ToListAsync();

            Customer customer = await _context.Customers.Include(t => t.CustomerCategory).Where(t => t.CustomerID == id).FirstOrDefaultAsync();

            ViewBag.Customer = customer;


            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "FinalSettlement_Customer_Create")]
        // GET: FinalSettlement_Customer/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name");
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", finalSettlement_Customer.CustomerID);
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", finalSettlement_Customer.CustomerID);
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", finalSettlement_Customer.CustomerID);
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
        public async Task<JsonResult> GetRemain(string goodsIssueID)
        {
            var model = await _context.Shop_Goods_Issues
                .Include(t => t.FinalSettlement_Customers)
                .Where(t => t.GoodsIssueID == goodsIssueID)
                .FirstOrDefaultAsync();

            return Json(new
            {
                data = new
                {
                    remain = model.Total - model.FinalSettlement_Customers.Select(t => t.Payment).Sum()
                }
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetByGoodsIssueID(string goodsIssueID)
        {
            var model = await _context.FinalSettlement_Customers
                .Where(t => t.GoodsIssuesID == goodsIssueID)
                .OrderByDescending(t => t.ID)
                .ToArrayAsync();

            return Json(new
            {
                data = model.Select(t => new
                {
                    DateCreated = t.DateCreated.Value.ToString("dd/MM/yyyy"),
                    Payment = t.Payment,
                    Remainder = t.Remainder
                })
            });
        }

        [Authorize(Roles = "FinalSettlement_Customer_Create")]
        [HttpPost]
        public async Task<JsonResult> Add(FinalSettlement_Customer info)
        {
            string msg = "ok";

            try
            {
                var model = await _context.Shop_Goods_Issues
                .Include(t => t.FinalSettlement_Customers)
                .Where(t => t.GoodsIssueID == info.GoodsIssuesID)
                .FirstOrDefaultAsync();
                info.Remainder = model.Total - info.Payment - model.FinalSettlement_Customers.Select(t => t.Payment).Sum();
                _context.FinalSettlement_Customers.Add(info);
                await _context.SaveChangesAsync();

            }
            catch
            {
                msg = "";
            }

            return Json(new
            {
                msg = msg
            });
        }
    }
}
