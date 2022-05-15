using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHouseManger.Models.EF;
using WareHouseManger.ViewModels;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    public class Shop_Goods_IssuesController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_IssuesController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Issues_Index")]
        // GET: shop_Goods_Issue
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_Issues
                .Include(s => s.Employee)
                .Include(s => s.Customer)
                .Where(t => t.GoodsIssueID.Contains(keyword) ||
                t.CustomerID.ToString().Contains(keyword) ||
                t.Customer.Name.Contains(keyword) ||
                t.EmployeeID.ToString().Contains(keyword) ||
                t.Employee.Name.Contains(keyword))
                .OrderByDescending(t => t.GoodsIssueID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }

        [Authorize(Roles = "Shop_Goods_Issues_Details")]
        // GET: shop_Goods_Issue/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Issue = await _context.Shop_Goods_Issues
                .Include(s => s.Employee)
                .Include(s => s.Customer)
                .Include(t => t.Shop_Goods_Issues_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.Producer)
                .Include(t => t.Shop_Goods_Issues_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.Unit)
                .Include(t => t.Shop_Goods_Issues_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.SubCategory)
                .FirstOrDefaultAsync(m => m.GoodsIssueID == id);

            if (shop_Goods_Issue == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Create")]
        // GET: shop_Goods_Issue/Create
        public async Task<IActionResult> Create()
        {
            var model = new Shop_Goods_Issue()
            {
                DateCreated = DateTime.Now
            };

            var category = await _context.Shop_Goods_SubCategories.ToListAsync();
            category.Insert(0, new Shop_Goods_SubCategory() { SubCategoryID = -1, SubCategoriName = "Tất cả" });
            ViewBag["CategoryID"] = new SelectList(category, "SubCategoryID", "SubCategoriName", -1);
            ViewData["EmployeeID"] = await _context.Employees.Where(t => !(bool)t.IsDelete).ToListAsync();
            ViewData["CustomerID"] = await _context.Customers.Where(t => !(bool)t.IsDelete).ToListAsync(); /*new SelectList(_context.Customers, "CustomerID", "Name");*/

            return View(model);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Create")]
        // POST: shop_Goods_Issue/Create
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Issue.EmployeeID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Edit")]
        // GET: shop_Goods_Issue/Edit/5
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Issue.EmployeeID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Edit")]
        // POST: shop_Goods_Issue/Edit/5
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
                    if (!shop_Goods_IssueExists(shop_Goods_Issue.GoodsIssueID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_Issue.EmployeeID);
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", shop_Goods_Issue.CustomerID);
            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Delete")]
        // GET: shop_Goods_Issue/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_Issue = await _context.Shop_Goods_Issues
                .Include(s => s.Employee)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.GoodsIssueID == id);
            if (shop_Goods_Issue == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Issue);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Delete")]
        // POST: shop_Goods_Issue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var shop_Goods_Issue = await _context.Shop_Goods_Issues.FindAsync(id);

                var shop_Goods_Issue_Details = await _context.Shop_Goods_Issues_Details.Where(t => t.GoodsIssueID == id).ToListAsync();

                var finalSettlement_Customers = await _context.FinalSettlement_Customers.Where(t => t.GoodsIssuesID == id).ToListAsync();

                _context.Shop_Goods_Issues_Details.RemoveRange(shop_Goods_Issue_Details);
                _context.FinalSettlement_Customers.RemoveRange(finalSettlement_Customers);
                _context.Shop_Goods_Issues.Remove(shop_Goods_Issue);

                await UpdateCount(shop_Goods_Issue_Details, 1);

                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool shop_Goods_IssueExists(string id)
        {
            return _context.Shop_Goods_Issues.Any(e => e.GoodsIssueID == id);
        }

        [Authorize(Roles = "Shop_Goods_Issues_Create")]
        [HttpPost]
        public async Task<JsonResult> CreateConfirmed(Shop_Goods_Issue info, string json)
        {
            string msg = "msg";
            string GoodsIssueID = "";
            try
            {
                List<Shop_Goods_Issues_Detail> shop_Goods_Issue_Details = JsonConvert.DeserializeObject<List<Shop_Goods_Issues_Detail>>(json);
                if (shop_Goods_Issue_Details.Count > 0)
                {
                    string name = "PX";
                    string maxID = await _context.Shop_Goods_Issues.MaxAsync(t => t.GoodsIssueID);

                    maxID = maxID == null ? "0" : maxID;

                    maxID = maxID.Replace(name, "").Trim();

                    int newID = int.Parse(maxID) + 1;

                    int length = 10 - 2 - newID.ToString().Length;

                    GoodsIssueID = name;

                    while (length > 0)
                    {
                        GoodsIssueID += "0";
                        length--;
                    }

                    GoodsIssueID += newID;

                    info.GoodsIssueID = GoodsIssueID;

                    foreach (Shop_Goods_Issues_Detail shop_Goods_Issue_Detail in shop_Goods_Issue_Details)
                    {
                        shop_Goods_Issue_Detail.GoodsIssueID = info.GoodsIssueID;
                    }

                    info.Shop_Goods_Issues_Details = shop_Goods_Issue_Details;
                    info.Total = info.Shop_Goods_Issues_Details.Select(t => (decimal)t.Count * t.UnitPrice).Sum();
                    info.DateCreated = DateTime.Now;

                    FinalSettlement_Customer finalSettlement_Customer = new FinalSettlement_Customer()
                    {
                        GoodsIssuesID = info.GoodsIssueID,
                        CustomerID = info.CustomerID,
                        Payment = info.Prepay,
                        Remainder = info.Total - info.Prepay,
                        DateCreated = info.DateCreated
                    };

                    info.FinalSettlement_Customers.Add(finalSettlement_Customer);

                    await _context.Shop_Goods_Issues.AddAsync(info);

                    await UpdateCount(shop_Goods_Issue_Details, -1);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    msg = "";
                }
            }
            catch (Exception ex)
            {
                msg = "";
            }

            return Json(new { msg = msg, id = GoodsIssueID });
        }

        private async Task UpdateCount(List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details, int num)
        {
            //update count 
            var templateIds = shop_Goods_Issues_Details.Select(t => t.TemplateID).ToArray();

            List<Shop_Good> shop_Goods = await _context.Shop_Goods.Where(t => templateIds.Contains(t.TemplateID)).ToListAsync();

            foreach (Shop_Good item in shop_Goods)
            {
                Shop_Goods_Issues_Detail shop_Goods_Issues_Detail = shop_Goods_Issues_Details.Where(t => t.TemplateID == item.TemplateID).FirstOrDefault();

                item.Count += shop_Goods_Issues_Detail.Count * num;
            }
        }
    }
}
