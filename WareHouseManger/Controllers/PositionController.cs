using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    public class PositionController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public PositionController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Position_Index")]
        // GET: Position
        public async Task<IActionResult> Index()
        {
            return View(await _context.Positions.ToListAsync());
        }

        [Authorize(Roles = "Position_Details")]
        // GET: Position/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        [Authorize(Roles = "Position_Create")]
        // GET: Position/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Position_Create")]
        // POST: Position/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionID,Name,Remark")] Position producer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        [Authorize(Roles = "Position_Edit")]
        // GET: Position/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Positions.FindAsync(id);
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }

        [Authorize(Roles = "Position_Edit")]
        // POST: Position/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PositionID,Name,Remark")] Position producer)
        {
            if (id != producer.PositionID)
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
                    if (!PositionExists(producer.PositionID))
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

        [Authorize(Roles = "Position_Delete")]
        // GET: Position/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Positions
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        [Authorize(Roles = "Position_Delete")]
        // POST: Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producer = await _context.Positions.FindAsync(id);
            _context.Positions.Remove(producer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.PositionID == id);
        }
    }
}
