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
    [Authorize]
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

            Shop_Goods_ClosingStock shop_Goods_ClosingStock = await _context.Shop_Goods_ClosingStocks.OrderBy(t => t.ClosingStockID).LastOrDefaultAsync();

            if (shop_Goods_ClosingStock == null)
            {
                shop_Goods_ClosingStock = new Shop_Goods_ClosingStock()
                {
                    DateClosing = new DateTime(),
                };

                ViewBag.DateClosingStock = "Chưa chốt sổ lần nào!";
            }
            else
            {
                ViewBag.DateClosingStock = shop_Goods_ClosingStock.DateClosing.Value.ToString("HH:mm:ss dd/MM/yyyy");
            }

            ViewBag.Shop_Goods_ClosingStock = shop_Goods_ClosingStock;

            return View(await _context.Shop_Goods
                .Include(s => s.Category)
                .Include(s => s.Producer)
                .Include(s => s.Unit)
                .Include(s => s.Shop_Goods_Issues_Details)
                .ThenInclude(s => s.GoodsIssue)
                .Include(s => s.Shop_Goods_Receipt_Details)
                .ThenInclude(s => s.GoodsReceipt)
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
                .Include(s => s.Shop_Goods_Issues_Details)
                .Include(s => s.Shop_Goods_Receipt_Details)
                .FirstOrDefaultAsync(m => m.TemplateID == id);

            Shop_Goods_ClosingStock shop_Goods_ClosingStock = await _context.Shop_Goods_ClosingStocks
                .OrderBy(t => t.ClosingStockID)
                .LastOrDefaultAsync();

            if (shop_Goods_ClosingStock == null)
            {
                shop_Goods_ClosingStock = new Shop_Goods_ClosingStock()
                {
                    DateClosing = new DateTime()
                };
            }


            List<StockCard> stockCards = await GetStockCardsAsync((DateTime)shop_Goods_ClosingStock.DateClosing, DateTime.Now, id);

            ViewData["DateBegin"] = shop_Goods_ClosingStock.DateClosing;
            ViewData["DateEnd"] = DateTime.Now;
            ViewBag.StockCards = stockCards;


            if (shop_Good == null)
            {
                return NotFound();
            }

            return View(shop_Good);
        }


        [HttpPost]
        public async Task<JsonResult> GetStockCard(DateTime dateBegin, DateTime dateEnd, string templateID)
        {
            return Json(new { date = await GetStockCardsAsync(dateBegin, dateEnd, templateID) });
        }

        [HttpGet]
        public async Task<IActionResult> ViewStockCard(DateTime dateBegin, DateTime dateEnd, string templateID)
        {
            return PartialView(await GetStockCardsAsync(dateBegin, dateEnd, templateID));
        }
        private async Task<List<StockCard>> GetStockCardsAsync(DateTime dateBegin, DateTime dateEnd, string templateID)
        {
            dateBegin = new DateTime(dateBegin.Year, dateBegin.Month, dateBegin.Day);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day).AddHours(24);

            //if (dateBegin == dateEnd)
            //{
            //    dateEnd = dateEnd.AddHours(24);
            //}

            List<StockCard> stockCards = new List<StockCard>();

            List<Shop_Goods_Issues_Detail> shop_Goods_Issues_Details = _context.Shop_Goods_Issues_Details.Count() > 0 ? await _context.Shop_Goods_Issues_Details
                .Include(t => t.GoodsIssue)
                .Where(t => t.TemplateID == templateID &&
                dateBegin <= t.GoodsIssue.DateCreated && t.GoodsIssue.DateCreated <= dateEnd)
                .ToListAsync() : new List<Shop_Goods_Issues_Detail>();

            List<Shop_Goods_Receipt_Detail> shop_Goods_Receipt_Details = _context.Shop_Goods_Receipt_Details.Count() > 0 ? await _context.Shop_Goods_Receipt_Details
                .Include(t => t.GoodsReceipt)
                .Where(t => t.TemplateID == templateID &&
                dateBegin <= t.GoodsReceipt.DateCreated && t.GoodsReceipt.DateCreated <= dateEnd)
                .ToListAsync() : new List<Shop_Goods_Receipt_Detail>();

            Shop_Goods_ClosingStock_Detail closingStockDetailBeforeDateBegin = _context.Shop_Goods_ClosingStock_Details.Count() > 0 ? await _context.Shop_Goods_ClosingStock_Details
                .Include(t => t.ClosingStock)
                .Where(t => t.ClosingStock.DateClosing <= dateBegin && t.TemplateID == templateID)
                .OrderBy(t => t.ClosingStockID)
                .LastOrDefaultAsync() : new Shop_Goods_ClosingStock_Detail()
                {
                    ClosingStockID = "",
                    ClosingStock = new Shop_Goods_ClosingStock()
                    {
                        DateClosing = new DateTime()
                    },
                    Count = 0
                };

            List<Shop_Goods_ClosingStock_Detail> closingStockDetailAfterDateBegin = _context.Shop_Goods_ClosingStock_Details.Count() > 0 ? await _context.Shop_Goods_ClosingStock_Details
                .Include(t => t.ClosingStock)
                .Where(t => t.ClosingStock.DateClosing > dateBegin && t.ClosingStock.DateClosing <= dateEnd && t.TemplateID == templateID)
                .OrderBy(t => t.ClosingStockID)
                .ToListAsync() : new List<Shop_Goods_ClosingStock_Detail>();

            List<Shop_Goods_StockTake_Detail> shop_Goods_StockTake_Details = _context.Shop_Goods_StockTake_Details.Count() > 0 ? await _context.Shop_Goods_StockTake_Details
                .Include(t => t.StockTake)
                .Where(t => t.TemplateID == templateID &&
                dateBegin <= t.StockTake.DateCreated && t.StockTake.DateCreated <= dateEnd)
                .ToListAsync() : new List<Shop_Goods_StockTake_Detail>();

            foreach (var item in shop_Goods_Issues_Details)
            {
                stockCards.Add(InitStockCard(item.GoodsIssueID, (DateTime)item.GoodsIssue.DateCreated, (int)eNumCardStockType.GoodsIsuess, (int)item.Count, (int)item.UnitPrice));
            }

            foreach (var item in shop_Goods_Receipt_Details)
            {
                stockCards.Add(InitStockCard(item.GoodsReceiptID, (DateTime)item.GoodsReceipt.DateCreated, (int)eNumCardStockType.GoodsReceipt, (int)item.Count, (int)item.UnitPrice));
            }

            foreach (var item in shop_Goods_StockTake_Details)
            {
                var valDiff = item.AmountOfStock - item.ActualAmount;

                if (valDiff > 0)
                {
                    stockCards.Add(InitStockCard(item.StockTakeID, (DateTime)item.StockTake.DateCreated, (int)eNumCardStockType.GoodsReceipt, (int)valDiff, 0));
                }
                else
                {
                    stockCards.Add(InitStockCard(item.StockTakeID, (DateTime)item.StockTake.DateCreated, (int)eNumCardStockType.GoodsIsuess, Math.Abs((int)valDiff), 0));
                }
            }

            if (closingStockDetailBeforeDateBegin == null)
            {
                stockCards.Add(InitStockCard("", new DateTime(), (int)eNumCardStockType.ClossingStock, 0, 0));
            }
            else
            {
                stockCards.Add(InitStockCard(closingStockDetailBeforeDateBegin.ClosingStockID,
                    (DateTime)closingStockDetailBeforeDateBegin.ClosingStock.DateClosing,
                    (int)eNumCardStockType.ClossingStock, (int)closingStockDetailBeforeDateBegin.Count, 0));
            }

            foreach (var item in closingStockDetailAfterDateBegin)
            {
                stockCards.Add(InitStockCard(item.ClosingStockID,
                    (DateTime)item.ClosingStock.DateClosing,
                    (int)eNumCardStockType.ClossingStock, (int)item.Count, 0));
            }

            stockCards = stockCards.OrderBy(t => t.DateCreated).ToList();

            return stockCards;
        }

        private StockCard InitStockCard(string id, DateTime dateCreated, int category, int count, int price)
        {
            StockCard stockCard = new StockCard()
            {
                ID = id,
                DateCreated = dateCreated,
                Category = category,
                Count = count,
                Price = price
            };

            return stockCard;
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
        public async Task<IActionResult> Create([Bind("TemplateID,Name,CategoryID,UnitID,Description,Price,CostPrice,ProducerID")] Shop_Good shop_Good)
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
                shop_Good.Count = 0;
                //shop_Good.CostPrice = 1;

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
        public async Task<IActionResult> Edit(string id, [Bind("TemplateID,Name,CategoryID,UnitID,Description,Price,CostPrice,ProducerID")] Shop_Good shop_Good)
        {
            if (id != shop_Good.TemplateID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var shop_Good0 = await _context.Shop_Goods.FindAsync(shop_Good.TemplateID);

                    shop_Good0.Name = shop_Good.Name;
                    shop_Good0.CategoryID = shop_Good.CategoryID;
                    shop_Good0.UnitID = shop_Good.UnitID;
                    shop_Good0.Description = shop_Good.Description;
                    shop_Good0.Price = shop_Good.Price;
                    shop_Good0.CostPrice = shop_Good.CostPrice;
                    shop_Good0.ProducerID = shop_Good.ProducerID;

                    //_context.Update(shop_Good);
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
            List<Shop_Good> templates = new List<Shop_Good>();

            if (categoryID != -1)
            {
                templates = await _context.Shop_Goods
                .Where(t => !templateIDs.Contains(t.TemplateID) && t.CategoryID == categoryID)
                .Include(t => t.Category)
                .Include(t => t.Unit)
                .Include(t => t.Producer)
                .ToListAsync();
            }
            else
            {
                templates = await _context.Shop_Goods
                .Where(t => !templateIDs.Contains(t.TemplateID))
                .Include(t => t.Category)
                .Include(t => t.Unit)
                .Include(t => t.Producer)
                .ToListAsync();
            }

            return Json(new
            {
                data = templates.Select(t => new
                {
                    id = t.TemplateID,
                    name = t.Name,
                    category = t.Category.Name,
                    price = t.Price,
                    costprice = t.CostPrice,
                    count = t.Count,
                    unit = t.Unit.Name,
                    producer = t.Producer.Name
                })
            });
        }
    }
}
