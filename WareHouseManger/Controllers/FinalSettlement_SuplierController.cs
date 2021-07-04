﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    public class FinalSettlement_SuplierController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public FinalSettlement_SuplierController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        // GET: FinalSettlement_Suplier
        public async Task<IActionResult> Index()
        {
            var dB_WareHouseMangerContext = _context.FinalSettlement_Supliers.Include(f => f.GoodsReceipt).Include(f => f.Supplier);
            return View(await dB_WareHouseMangerContext.ToListAsync());
        }

        // GET: FinalSettlement_Suplier/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: FinalSettlement_Suplier/Create
        public IActionResult Create()
        {
            ViewData["GoodsReceiptID"] = new SelectList(_context.Shop_Goods_Receipts, "GoodsReceiptID", "GoodsReceiptID");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID");
            return View();
        }

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
    }
}