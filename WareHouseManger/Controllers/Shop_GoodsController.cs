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
    public class Shop_GoodsController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_GoodsController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Shop_Goods_Index")]
        // GET: Shop_Goods
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Shop_Goods
                .Include(s => s.Category)
                .Include(s => s.Producer)
                .Include(s => s.Unit)
                .Where(t => t.TemplateID.Contains(keyword) || t.Name.Contains(keyword))
                .OrderByDescending(t => t.TemplateID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize(Roles = "Shop_Goods_Details")]
        // GET: Shop_Goods/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Good = await _context.Shop_Goods
                .Include(s => s.Category)
                .Include(s => s.Producer)
                .Include(s => s.Unit)
                .FirstOrDefaultAsync(m => m.TemplateID == id);
            if (shop_Good == null)
            {
                return NotFound();
            }

            return View(shop_Good);
        }

        [Authorize(Roles = "Shop_Goods_Create")]
        // GET: Shop_Goods/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name");
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ProducerID", "Name");
            ViewData["UnitID"] = new SelectList(_context.Shop_Goods_Units, "UnitID", "Name");
            return View();
        }

        [Authorize(Roles = "Shop_Goods_Create")]
        // POST: Shop_Goods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemplateID,Name,CategoryID,UnitID,Description,Price,Count,CountMin,ProducerID")] Shop_Good shop_Good)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.Shop_Goods_Categories.FindAsync(shop_Good.CategoryID);

                string maxID = await _context.Shop_Goods.Where(t => t.TemplateID.Contains(category.SortName.Trim())).MaxAsync(t => t.TemplateID);

                maxID = maxID == null ? "0" : maxID;

                maxID = maxID.Replace(category.SortName.Trim(), "").Trim();

                int newID = int.Parse(maxID) + 1;

                int length = 10 - category.SortName.Trim().Length - newID.ToString().Length;

                string templateID = category.SortName.Trim();

                while (length > 0)
                {
                    templateID += "0";
                    length--;
                }

                templateID += newID;

                shop_Good.TemplateID = templateID;

                _context.Add(shop_Good);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name", shop_Good.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ProducerID", "Name", shop_Good.ProducerID);
            ViewData["UnitID"] = new SelectList(_context.Shop_Goods_Units, "UnitID", "Name", shop_Good.UnitID);
            return View(shop_Good);
        }

        [Authorize(Roles = "Shop_Goods_Edit")]
        // GET: Shop_Goods/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Good = await _context.Shop_Goods.FindAsync(id);
            if (shop_Good == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name", shop_Good.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ProducerID", "Name", shop_Good.ProducerID);
            ViewData["UnitID"] = new SelectList(_context.Shop_Goods_Units, "UnitID", "Name", shop_Good.UnitID);
            return View(shop_Good);
        }

        [Authorize(Roles = "Shop_Goods_Edit")]
        // POST: Shop_Goods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TemplateID,Name,CategoryID,UnitID,Description,Price,Count,CountMin,ProducerID")] Shop_Good shop_Good)
        {
            if (id != shop_Good.TemplateID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop_Good);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Shop_GoodExists(shop_Good.TemplateID))
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
            ViewData["CategoryID"] = new SelectList(_context.Shop_Goods_Categories, "CategoryID", "Name", shop_Good.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ProducerID", "Name", shop_Good.ProducerID);
            ViewData["UnitID"] = new SelectList(_context.Shop_Goods_Units, "UnitID", "Name", shop_Good.UnitID);
            return View(shop_Good);
        }

        [Authorize(Roles = "Shop_Goods_Delete")]
        // GET: Shop_Goods/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop_Good = await _context.Shop_Goods
                .Include(s => s.Category)
                .Include(s => s.Producer)
                .Include(s => s.Unit)
                .FirstOrDefaultAsync(m => m.TemplateID == id);
            if (shop_Good == null)
            {
                return NotFound();
            }

            return View(shop_Good);
        }

        [Authorize(Roles = "Shop_Goods_Delete")]
        // POST: Shop_Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var shop_Good = await _context.Shop_Goods.FindAsync(id);
                _context.Shop_Goods.Remove(shop_Good);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        private bool Shop_GoodExists(string id)
        {
            return _context.Shop_Goods.Any(e => e.TemplateID == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetAnother(string templateIDs, int categoryID)
        {
            var templates = await _context.Shop_Goods
                .Where(t => !templateIDs.Contains(t.TemplateID) && t.CategoryID == categoryID)
                .Include(t => t.Category)
                .Include(t => t.Unit)
                .Include(t => t.Producer)
                .ToArrayAsync();

            return Json(new
            {
                data = templates.Select(t => new
                {
                    id = t.TemplateID,
                    name = t.Name,
                    category = t.Category.Name,
                    price = t.Price,
                    count = t.Count,
                    unit = t.Unit.Name,
                    producer = t.Producer.Name
                })
            });
        }
    }
}
