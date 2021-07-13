using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WareHouseManger.Common;
using WareHouseManger.Models.EF;

namespace WareHouseManger.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;

        public EmployeeController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,PhoneNumber,Address,EMail")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Name,PhoneNumber,Address,EMail")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }

        public async Task<IActionResult> ActiveAcountConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee.Accounts.Count == 0)
            {
                Account account = new Account();

                string name = "NV";
                string maxID = await _context.Accounts.MaxAsync(t => t.UserName);

                maxID = maxID == null ? "0" : maxID;

                maxID = maxID.Replace(name, "").Trim();

                int newID = int.Parse(maxID) + 1;

                int length = 10 - 2 - newID.ToString().Length;

                string userName = name;

                while (length > 0)
                {
                    userName += "0";
                    length--;
                }

                userName += newID;

                account.UserName = userName;
                account.Password = MD5.CreateHash("123456");
                account.StatusID = 1;
                account.DateCreated = DateTime.Now;
                account.EmployeeID = employee.EmployeeID;

                await _context.AddAsync(account);

            }

            return RedirectToAction("Role", new { id = id });
        }

        public async Task<IActionResult> Role(int id)
        {
            var roles = await _context.Account_Role_Details
                .Where(t => t.AccountID == id)
                .Select(t => t.Role.Name)
                .ToArrayAsync();

            ViewData["Roles"] = roles;

            var roleGroups = await _context.RoleGroups.Include(t => t.Roles).ToArrayAsync();


            return View(roleGroups);
        }
    }
}
