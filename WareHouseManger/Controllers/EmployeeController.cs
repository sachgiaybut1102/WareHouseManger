using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WareHouseManger.Common;
using WareHouseManger.Models.EF;
using X.PagedList;

namespace WareHouseManger.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeController(DB_WareHouseMangerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize(Roles = "Employee_Index")]
        // GET: Employee
        public async Task<IActionResult> Index(int? page, string keyword)
        {
            int currentPage = (int)(page != null ? page : 1);

            keyword = keyword != null ? keyword : "";

            ViewBag.Keyword = keyword;

            return View(await _context.Employees
                .Include(t => t.Accounts)
                .Where(t => t.EmployeeID.ToString().Contains(keyword) ||
                t.Name.Contains(keyword) ||
                t.PhoneNumber.Contains(keyword) ||
                t.EMail.Contains(keyword))
                .OrderByDescending(t => t.EmployeeID)
                .ToList()
                .ToPagedListAsync(currentPage, 10));
        }


        [Authorize/*(Roles = "Employee_Details")*/]
        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            bool isSingle = false;

            if (id == null)
            {
                string value = User.Claims.Where(t => t.Type == "AccountID").Select(t => t.Value).FirstOrDefault();

                Account account = await _context.Accounts.FindAsync(int.Parse(value));

                if (account != null)
                {
                    id = account.EmployeeID;
                    isSingle = true;
                }
                else
                {
                    return NotFound();
                }
            }

            var employee = await _context.Employees
                .Include(t => t.Accounts)
                .Include(t => t.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);


            if (employee == null)
            {
                return NotFound();
            }

            ViewData["IsSingle"] = isSingle;

            return View(employee);
        }

        [Authorize(Roles = "Employee_Create")]
        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = new SelectList(await _context.Positions.ToListAsync(), "PositionID", "Name");

            return View();
        }

        [Authorize(Roles = "Employee_Create")]
        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,PhoneNumber,Address,EMail,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Position = new SelectList(await _context.Positions.ToListAsync(), "PositionID", "Name", employee.PositionID);
            return View(employee);
        }

        [Authorize(Roles = "Employee_Edit")]
        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            bool isSingle = false;

            if (id == null)
            {
                string value = User.Claims.Where(t => t.Type == "AccountID").Select(t => t.Value).FirstOrDefault();

                Account account = await _context.Accounts.FindAsync(int.Parse(value));

                if (account != null)
                {
                    id = account.EmployeeID;
                    isSingle = true;
                }
                else
                {
                    return NotFound();
                }
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Position = new SelectList(await _context.Positions.ToListAsync(), "PositionID", "Name", employee.PositionID);
            ViewData["IsSingle"] = isSingle;
            return View(employee);
        }

        [Authorize(Roles = "Employee_Edit")]
        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Name,PhoneNumber,Address,EMail,PositionID")] Employee employee)
        {
            bool isSingle = false;

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

                string value = User.Claims.Where(t => t.Type == "AccountID").Select(t => t.Value).FirstOrDefault();

                Account account = await _context.Accounts.FindAsync(int.Parse(value));

                if (account != null)
                {
                    id = (int)account.EmployeeID;
                    isSingle = true;
                }

                if (isSingle)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index), "Home");
                }
            }

            ViewBag.Position = new SelectList(await _context.Positions.ToListAsync(), "PositionID", "Name", employee.PositionID);
            ViewData["IsSingle"] = isSingle;

            return View(employee);
        }

        [Authorize(Roles = "Employee_Delete")]
        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                 .Include(t => t.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [Authorize(Roles = "Employee_Delete")]
        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(t => t.Position)
                    .FirstOrDefaultAsync(m => m.EmployeeID == id);

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }

        [Authorize(Roles = "Account_Create")]
        public async Task<IActionResult> ActiveAcountConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee.Accounts.Count == 0)
            {
                Account account = new Account();

                string name = "NV";
                string maxID = await _context.Accounts.MaxAsync(t => t.UserName);

                maxID = maxID == null ? "0" : maxID.Trim() == "admin" ? "0" : maxID;

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
                account.Password = MD5.CreateHash(_configuration["DefaultPasswod:Value"]);
                account.StatusID = 1;
                account.DateCreated = DateTime.Now;
                account.EmployeeID = employee.EmployeeID;

                await _context.AddAsync(account);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Role", new { id = id });
        }

        [Authorize(Roles = "Account_Edit")]
        public async Task<IActionResult> Role(int id)
        {

            var roles = await _context.Account_Role_Details
                    .Where(t => t.AccountID == id)
                    .Select(t => t.Role.Name)
                    .ToArrayAsync();

            ViewData["Roles"] = roles;
            ViewData["EmployeeID"] = id;

            var roleGroups = await _context.RoleGroups.Include(t => t.Roles).ToArrayAsync();


            return View(roleGroups);
        }
    }
}
