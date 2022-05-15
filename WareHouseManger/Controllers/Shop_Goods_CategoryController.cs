using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    [Authorize]
    public class Shop_Goods_CategoryController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public Shop_Goods_CategoryController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            var categoris = await _context.Shop_Goods_Categories.Include(x => x.Shop_Goods_SubCategories).Where(x => x.Name.Contains(keyword)).ToPagedListAsync(currentPage, 10);
            return View(categoris);
        }

        // GET: ProduShop_Goods_Categorycer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shop_Goods_Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Name,SortName")] Shop_Goods_Category category)
        {
            if (ModelState.IsValid)
            {
                //category.IsDelete = false;    
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Shop_Goods_Category/Create
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categori = await _context.Shop_Goods_Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
            if (categori == null)
            {
                return NotFound();
            }
            return View(categori);
        }


        // POST: Shop_Goods_Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,Name,SortName")] Shop_Goods_Category categori)
        {
            if (id != categori.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //categori.IsDelete = false;
                    _context.Update(categori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Shop_Goods_Categories.AnyAsync(x => x.CategoryID == id))
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
            return View(categori);
        }



        // GET: Shop_Goods_Category/Delete
        public async Task<IActionResult> Detail(int? id)
        {
            var detail = await _context.Shop_Goods_Categories.FirstOrDefaultAsync(m => m.CategoryID == id);

            return Json(new
            {
                data = new
                {
                    id = detail.CategoryID,
                    name = detail.Name
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            try
            {
                var cate = await _context.Shop_Goods_Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
                if (cate == null)
                    return Json(NotFound());
                else
                {
                    _context.Shop_Goods_Categories.Remove(cate);
                    await _context.SaveChangesAsync();
                    return Json(new { succses = true, });
                }
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSubCateByCateParentId(int parentId)
        {
            List<Shop_Goods_SubCategory> subCates = null;
            if (parentId == 0)
            {
                return Json(new { succes = false, data = subCates, cateParentName = "" });
            }
            else
            {
                subCates = await _context.Shop_Goods_SubCategories.Where(subCa => subCa.CategoryParentID == parentId).ToListAsync();
                var cate = await _context.Shop_Goods_Categories.FirstOrDefaultAsync(cate => cate.CategoryID == parentId);
                if (subCates.Count == 0 && cate != null)
                {
                    return Json(new { succes = false, data = subCates, cateParentName = cate.Name });
                }
                else if (cate != null)
                    return Json(new
                    {
                        succes = true,
                        data = subCates.Select(subCate => new
                        {
                            id = subCate.SubCategoryID,
                            name = subCate.SubCategoriName,
                            descrip = subCate.SubCategoriDescription,
                            subname = subCate.SortName
                        }),
                        cateParentName = cate.Name
                    });
                else
                    return Json(new { succes = false, data = subCates, cateParentName = "" });
            }
        }

        public async Task<IActionResult> CreateSubCate(int? parentId)
        {
            ViewBag.ParentId = parentId;
            var parentCate = await _context.Shop_Goods_Categories.Where(x=>x.CategoryID == parentId).FirstOrDefaultAsync();
            if(parentCate == null)
                return RedirectToAction(nameof(Index));
            ViewBag.ParentName = parentCate.Name;
            return View();
        }

        // POST: Shop_Goods_Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCate([Bind("CategoryParentID,SubCategoriName,SubCategoriDescription,SortName")] Shop_Goods_SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                //category.IsDelete = false;    
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subCategory);
        }

        // GET: Shop_Goods_Category/EditSubCate
        public async Task<IActionResult> EditSubCate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categori = await _context.Shop_Goods_SubCategories.FirstOrDefaultAsync(x => x.SubCategoryID == id);
            if (categori == null)
            {
                return NotFound();
            }
            ViewBag.Parent = await _context.Shop_Goods_Categories.ToListAsync();
            return View(categori);
        }


        // POST: Shop_Goods_Category/EditSubCate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubCate(int id, [Bind("SubCategoryID,CategoryParentID,SubCategoriName,SubCategoriDescription,SortName")] Shop_Goods_SubCategory subCategory)
        {
            if (id != subCategory.SubCategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //categori.IsDelete = false;
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Shop_Goods_Categories.AnyAsync(x => x.CategoryID == id))
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
            return View(subCategory);
        }
        // GET: Shop_Goods_Category/Delete
        public async Task<IActionResult> DetailSubCate(int? id)
        {
            var detail = await _context.Shop_Goods_SubCategories.FirstOrDefaultAsync(m => m.SubCategoryID == id);

            return Json(new
            {
                data = new
                {
                    id = detail.SubCategoryID,
                    name = detail.SubCategoriName
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteConfirmedSubCate(int id)
        {
            try
            {
                var cate = await _context.Shop_Goods_SubCategories.FirstOrDefaultAsync(x => x.SubCategoryID == id);
                if (cate == null)
                    return Json(NotFound());
                else
                {
                    _context.Shop_Goods_SubCategories.Remove(cate);
                    await _context.SaveChangesAsync();
                    return Json(new { succses = true, });
                }
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex.Message));
            }
        }
    }
}
