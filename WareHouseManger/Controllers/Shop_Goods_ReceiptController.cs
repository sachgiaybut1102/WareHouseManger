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
    public class Shop_Goods_ReceiptController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_ReceiptController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Receipt_Index")]
        // GET: Shop_Goods_Receipt
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods_Receipts
                .Include(s => s.Employee)
                .Include(s => s.Supplier)
                .Where(t => t.GoodsReceiptID.Contains(keyword) ||
                t.SupplierID.ToString().Contains(keyword) ||
                t.Supplier.Name.Contains(keyword) ||
                t.EmployeeID.ToString().Contains(keyword) ||
                t.Employee.Name.Contains(keyword))
                .OrderByDescending(t => t.GoodsReceiptID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }

        [Authorize(Roles = "Shop_Goods_Receipt_Details")]
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
                .Include(t => t.Shop_Goods_Receipt_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.Producer)
                .Include(t => t.Shop_Goods_Receipt_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.Unit)
                .Include(t => t.Shop_Goods_Receipt_Details)
                    .ThenInclude(t => t.Template)
                        .ThenInclude(t => t.SubCategory)
                .FirstOrDefaultAsync(m => m.GoodsReceiptID == id);

            if (shop_Goods_Receipt == null)
            {
                return NotFound();
            }

            return View(shop_Goods_Receipt);
        }

        [Authorize(Roles = "Shop_Goods_Receipt_Create")]
        // GET: Shop_Goods_Receipt/Create
        public async Task<IActionResult> Create()
        {
            var model = new Shop_Goods_Receipt()
            {
                DateCreated = DateTime.Now
            };
            var category = await _context.Shop_Goods_SubCategories.ToListAsync();
            category.Insert(0, new Shop_Goods_SubCategory() { SubCategoryID = -1, SubCategoriName = "Tất cả" });
            ViewData["CategoryID"] = new SelectList(category, "SubCategoryID", "SubCategoriName", -1);
            ViewData["EmployeeID"] = await _context.Employees.Where(t => !(bool)t.IsDelete).ToListAsync();
            ViewData["SupplierID"] = await _context.Suppliers.Where(t => !(bool)t.IsDelete).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Shop_Goods_Receipt_Create")]
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

        [Authorize(Roles = "Shop_Goods_Receipt_Edit")]
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

        [Authorize(Roles = "Shop_Goods_Receipt_Edit")]
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

        [Authorize(Roles = "Shop_Goods_Receipt_Delete")]
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

        [Authorize(Roles = "Shop_Goods_Receipt_Delete")]
        // POST: Shop_Goods_Receipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var shop_Goods_Receipt = await _context.Shop_Goods_Receipts.FindAsync(id);

                var shop_Goods_Receipt_Details = await _context.Shop_Goods_Receipt_Details.Where(t => t.GoodsReceiptID == id).ToListAsync();

                var finalSettlement_Supliers = await _context.FinalSettlement_Supliers.Where(t => t.GoodsReceiptID == id).ToListAsync();

                _context.Shop_Goods_Receipt_Details.RemoveRange(shop_Goods_Receipt_Details);
                _context.FinalSettlement_Supliers.RemoveRange(finalSettlement_Supliers);
                _context.Shop_Goods_Receipts.Remove(shop_Goods_Receipt);

                await UpdateCount(shop_Goods_Receipt_Details, -1);

                await _context.SaveChangesAsync();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }

        private bool Shop_Goods_ReceiptExists(string id)
        {
            return _context.Shop_Goods_Receipts.Any(e => e.GoodsReceiptID == id);
        }

        [Authorize(Roles = "Shop_Goods_Receipt_Create")]
        [HttpPost]
        public async Task<JsonResult> CreateConfirmed(Shop_Goods_Receipt info, string json)
        {
            string msg = "msg";
            string goodsReceiptID = "";
            try
            {
                List<Shop_Goods_Receipt_Detail> shop_Goods_Receipt_Details = JsonConvert.DeserializeObject<List<Shop_Goods_Receipt_Detail>>(json);
                if (shop_Goods_Receipt_Details.Count > 0)
                {

                    string name = "PN";
                    string maxID = await _context.Shop_Goods_Receipts.MaxAsync(t => t.GoodsReceiptID);

                    maxID = maxID == null ? "0" : maxID;

                    maxID = maxID.Replace(name, "").Trim();

                    int newID = int.Parse(maxID) + 1;

                    int length = 10 - 2 - newID.ToString().Length;

                    goodsReceiptID = name;

                    while (length > 0)
                    {
                        goodsReceiptID += "0";
                        length--;
                    }

                    goodsReceiptID += newID;

                    info.GoodsReceiptID = goodsReceiptID;

                    foreach (Shop_Goods_Receipt_Detail shop_Goods_Receipt_Detail in shop_Goods_Receipt_Details)
                    {
                        shop_Goods_Receipt_Detail.GoodsReceiptID = info.GoodsReceiptID;
                    }

                    info.Shop_Goods_Receipt_Details = shop_Goods_Receipt_Details;
                    info.Total = info.Shop_Goods_Receipt_Details.Select(t => (decimal)t.Count * t.UnitPrice).Sum();
                    info.DateCreated = DateTime.Now;

                    FinalSettlement_Suplier finalSettlement_Suplier = new FinalSettlement_Suplier()
                    {
                        GoodsReceiptID = info.GoodsReceiptID,
                        SupplierID = info.SupplierID,
                        Payment = info.Prepay,
                        Remainder = info.Total - info.Prepay,
                        DateCreated = info.DateCreated
                    };

                    info.FinalSettlement_Supliers.Add(finalSettlement_Suplier);

                    await _context.Shop_Goods_Receipts.AddAsync(info);

                    await UpdateCount(shop_Goods_Receipt_Details, 1);

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


            return Json(new { msg = msg, id = goodsReceiptID });
        }

        private async Task UpdateCount(List<Shop_Goods_Receipt_Detail> shop_Goods_Receipt_Details, int num)
        {
            //update count 
            var templateIds = shop_Goods_Receipt_Details.Select(t => t.TemplateID).ToArray();

            List<Shop_Good> shop_Goods = await _context.Shop_Goods.Where(t => templateIds.Contains(t.TemplateID)).ToListAsync();

            foreach (Shop_Good item in shop_Goods)
            {
                Shop_Goods_Receipt_Detail shop_Goods_Receipt_Detail = shop_Goods_Receipt_Details.Where(t => t.TemplateID == item.TemplateID).FirstOrDefault();

                item.Count += shop_Goods_Receipt_Detail.Count * num;
            }
        }
    }
}
