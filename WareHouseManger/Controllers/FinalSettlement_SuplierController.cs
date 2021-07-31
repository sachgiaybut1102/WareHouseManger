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
    public class FinalSettlement_SuplierController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public FinalSettlement_SuplierController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Index")]
        // GET: FinalSettlement_Suplier
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Suppliers
                .Include(t => t.FinalSettlement_Supliers)
                .Include(t => t.Shop_Goods_Receipts)
                .Where(t => t.Name.Contains(keyword))
                .OrderByDescending(t => t.SupplierID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "FinalSettlement_Suplier_Details")]
        // GET: FinalSettlement_Suplier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Shop_Goods_Receipt> shop_Goods_Receipts = await _context.Shop_Goods_Receipts
                 .Where(t => t.SupplierID == id)
                 .Include(t => t.FinalSettlement_Supliers)
                 .Include(t => t.Employee)
                 .OrderByDescending(t => t.GoodsReceiptID)
                 .ToListAsync();

            Supplier supplier = await _context.Suppliers.Where(t => t.SupplierID == id).FirstOrDefaultAsync();

            ViewBag.Supplier = supplier;

            return View(shop_Goods_Receipts);
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Create")]
        // GET: FinalSettlement_Suplier/Create
        public IActionResult Create()
        {
            ViewData["GoodsReceiptID"] = new SelectList(_context.Shop_Goods_Receipts, "GoodsReceiptID", "GoodsReceiptID");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID");
            return View();
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Create")]
        // POST: FinalSettlement_Suplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SupplierID,GoodsReceiptID,DateCreated,Payment,Remainder")] FinalSettlement_Suplier finalSettlement_Suplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finalSettlement_Suplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GoodsReceiptID"] = new SelectList(_context.Shop_Goods_Receipts, "GoodsReceiptID", "GoodsReceiptID", finalSettlement_Suplier.GoodsReceiptID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", finalSettlement_Suplier.SupplierID);
            return View(finalSettlement_Suplier);
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Edit")]
        // GET: FinalSettlement_Suplier/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalSettlement_Suplier = await _context.FinalSettlement_Supliers.FindAsync(id);
            if (finalSettlement_Suplier == null)
            {
                return NotFound();
            }
            ViewData["GoodsReceiptID"] = new SelectList(_context.Shop_Goods_Receipts, "GoodsReceiptID", "GoodsReceiptID", finalSettlement_Suplier.GoodsReceiptID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", finalSettlement_Suplier.SupplierID);
            return View(finalSettlement_Suplier);
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Edit")]
        // POST: FinalSettlement_Suplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SupplierID,GoodsReceiptID,DateCreated,Payment,Remainder")] FinalSettlement_Suplier finalSettlement_Suplier)
        {
            if (id != finalSettlement_Suplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finalSettlement_Suplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinalSettlement_SuplierExists(finalSettlement_Suplier.ID))
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
            ViewData["GoodsReceiptID"] = new SelectList(_context.Shop_Goods_Receipts, "GoodsReceiptID", "GoodsReceiptID", finalSettlement_Suplier.GoodsReceiptID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", finalSettlement_Suplier.SupplierID);
            return View(finalSettlement_Suplier);
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Delete")]
        // GET: FinalSettlement_Suplier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalSettlement_Suplier = await _context.FinalSettlement_Supliers
                .Include(f => f.GoodsReceipt)
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (finalSettlement_Suplier == null)
            {
                return NotFound();
            }

            return View(finalSettlement_Suplier);
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Delete")]
        // POST: FinalSettlement_Suplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var finalSettlement_Suplier = await _context.FinalSettlement_Supliers.FindAsync(id);
            _context.FinalSettlement_Supliers.Remove(finalSettlement_Suplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinalSettlement_SuplierExists(int id)
        {
            return _context.FinalSettlement_Supliers.Any(e => e.ID == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetRemain(string goodsReceiptID)
        {
            var model = await _context.Shop_Goods_Receipts
                .Include(t => t.FinalSettlement_Supliers)
                .Where(t => t.GoodsReceiptID == goodsReceiptID)
                .FirstOrDefaultAsync();

            return Json(new
            {
                data = new
                {
                    remain = model.Total - model.FinalSettlement_Supliers.Select(t => t.Payment).Sum()
                }
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetByGoodsReceiptID(string goodsReceiptID)
        {
            var model = await _context.FinalSettlement_Supliers
                .Where(t => t.GoodsReceiptID == goodsReceiptID)
                .OrderByDescending(t => t.ID)
                .ToArrayAsync();

            return Json(new
            {
                data = model.Select(t => new
                {
                    DateCreated = t.DateCreated.Value.ToString("dd/MM/yyyy"),
                    Payment = t.Payment,
                    Remainder = t.Remainder,
                    Remark = t.Remark == null ? "" : t.Remark
                })
            });
        }

        [Authorize(Roles = "FinalSettlement_Suplier_Create")]
        [HttpPost]
        public async Task<JsonResult> Add(FinalSettlement_Suplier info)
        {
            string msg = "ok";

            try
            {
                var model = await _context.Shop_Goods_Receipts
                .Include(t => t.FinalSettlement_Supliers)
                .Where(t => t.GoodsReceiptID == info.GoodsReceiptID)
                .FirstOrDefaultAsync();

                info.Remainder = model.Total - info.Payment - model.FinalSettlement_Supliers.Select(t => t.Payment).Sum();

                _context.FinalSettlement_Supliers.Add(info);
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
