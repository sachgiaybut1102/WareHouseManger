using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    public class Shop_Goods_StockTakeController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_StockTakeController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Index")]
        // GET: Shop_Goods_StockTake
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_StockTakes
                .Include(t => t.Employee)
                .Where(t => t.StockTakeID.Contains(keyword) || t.Employee.Name.Contains(keyword))
                .OrderByDescending(t => t.StockTakeID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "Shop_Goods_StockTake_Details")]
        // GET: Shop_Goods_StockTake/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes
                .Include(s => s.Employee)
                .Include(t => t.Shop_Goods_StockTake_Details)
                .ThenInclude(t => t.Template.Category)
                .Include(t => t.Shop_Goods_StockTake_Details)
                .ThenInclude(t => t.Template.Producer)
                .Include(t => t.Shop_Goods_StockTake_Details)
                .ThenInclude(t => t.Template.Unit)

                .FirstOrDefaultAsync(m => m.StockTakeID == id);
            if (shop_Goods_StockTake == null)
            {
                return NotFound();
            }

            return View(shop_Goods_StockTake);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Create")]
        // GET: Shop_Goods_StockTake/Create
        public IActionResult Create()
        {
            var model = new Shop_Goods_StockTake()
            {
                DateCreated = DateTime.Now
            };

            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name");

            return View(model);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Create")]
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Edit")]
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Edit")]
        // POST: Shop_Goods_StockTake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StockTakeID,DateCreated,Remark,EmployeeID")] Shop_Goods_StockTake shop_Goods_StockTake)
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Name", shop_Goods_StockTake.EmployeeID);
            return View(shop_Goods_StockTake);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Delete")]
        // GET: Shop_Goods_StockTake/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        [Authorize(Roles = "Shop_Goods_StockTake_Delete")]
        // POST: Shop_Goods_StockTake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var shop_Goods_StockTake = await _context.Shop_Goods_StockTakes.FindAsync(id);
                _context.Shop_Goods_StockTakes.Remove(shop_Goods_StockTake);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_StockTakeExists(string id)
        {
            return _context.Shop_Goods_StockTakes.Any(e => e.StockTakeID == id);
        }

        [Authorize(Roles = "Shop_Goods_StockTake_Create")]
        [HttpPost]
        public async Task<JsonResult> CreateConfirmed(Shop_Goods_StockTake info, string json)
        {
            string msg = "msg";

            try
            {
                string name = "PK";
                string maxID = await _context.Shop_Goods_StockTakes.MaxAsync(t => t.StockTakeID);

                maxID = maxID == null ? "0" : maxID;

                maxID = maxID.Replace(name, "").Trim();

                int newID = int.Parse(maxID) + 1;

                int length = 10 - 2 - newID.ToString().Length;

                string stockTakeID = name;

                while (length > 0)
                {
                    stockTakeID += "0";
                    length--;
                }

                stockTakeID += newID;

                info.StockTakeID = stockTakeID;

                List<Shop_Goods_StockTake_Detail> shop_Goods_StockTake_Details = JsonConvert.DeserializeObject<List<Shop_Goods_StockTake_Detail>>(json);

                foreach (Shop_Goods_StockTake_Detail shop_Goods_StockTake_Detail in shop_Goods_StockTake_Details)
                {
                    shop_Goods_StockTake_Detail.StockTakeID = info.StockTakeID;
                }

                info.Shop_Goods_StockTake_Details = shop_Goods_StockTake_Details;

                await _context.Shop_Goods_StockTakes.AddAsync(info);

                //await UpdateCount(Shop_Goods_StockTake_Details, 1);

                await _context.SaveChangesAsync();

                //Update Count ShopGoods
                foreach (Shop_Goods_StockTake_Detail shop_Goods_StockTake_Detail in shop_Goods_StockTake_Details)
                {
                    var template = await _context.Shop_Goods.FindAsync(shop_Goods_StockTake_Detail.TemplateID);

                    template.Count += shop_Goods_StockTake_Detail.AmountOfStock - shop_Goods_StockTake_Detail.ActualAmount;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                msg = "";
            }

            return Json(new { msg = msg });
        }

    }
}
