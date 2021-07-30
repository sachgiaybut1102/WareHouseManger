using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WareHouseManger.Models.EF;
using WareHouseManger.ViewModels;

namespace WareHouseManger.Controllers
{
    public class AccountController : Controller
    {
        private readonly DB_WareHouseMangerContext _context;
        public AccountController(DB_WareHouseMangerContext context)
        {
            _context = context;
        }


        public IActionResult Login()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("User,Password")] AccountViewModel account)
        {
            account.Password = Common.MD5.CreateHash(account.Password);

            var model = await _context.Accounts
                .Where(t => t.UserName == account.User && t.Password == account.Password)
                .FirstOrDefaultAsync();

            if (model != null)
            {
                var roles = await _context.Account_Role_Details
                    .Where(t => t.AccountID == model.AccountID)
                    .Include(t => t.Role)
                    .Select(t => t.Role.Name)
                    .ToArrayAsync();

                List<Claim> claims = new List<Claim>();

                foreach (var role in roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role);

                    claims.Add(claim);
                }

                claims.Add(new Claim("AccountID", model.AccountID.ToString()));
                claims.Add(new Claim("UserName", model.UserName));

                #region Configuration Auth
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                #endregion

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["Msg"] = "Sai tài khoản hoặc mật khẩu, vui lòng đăng nhập lại";
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Account_Edit")]
        [HttpGet]
        public async Task<JsonResult> UpdateRole(int roleId)
        {
            var id = int.Parse(User.Claims.FirstOrDefault(t => t.Type.Equals("AccountID")).Value.ToString());
            string msg = "";

            if (await _context.Roles.FindAsync(roleId) != null)
            {
                var detail = await _context.Account_Role_Details.Where(t => t.AccountID == id && t.RoleID == roleId).FirstOrDefaultAsync();

                if (detail != null)
                {
                    _context.Remove(detail);
                }
                else
                {
                    await _context.Account_Role_Details.AddAsync(new Account_Role_Detail()
                    {
                        AccountID = id,
                        RoleID = roleId,
                    });
                }

                await _context.SaveChangesAsync();
                msg = "OK";
            }

            return Json(new { msg = msg });
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> ChangePassword(int id, string username, string str0, string str1, string str2)
        {
            bool flag = true;
            string msg = "Thay đổi thành công!";

            str0 = Common.MD5.CreateHash(str0.Trim());
            str1 = Common.MD5.CreateHash(str1.Trim());
            str2 = Common.MD5.CreateHash(str2.Trim());

            var employee = await _context.Employees.Include(t => t.Accounts).Where(t => t.EmployeeID == id).FirstOrDefaultAsync();

            if (employee != null)
            {
                Account account = employee.Accounts.FirstOrDefault();

                if (account != null)
                {
                    if (account.UserName.Trim() == username.Trim())
                    {
                        if (account.Password.Trim() == str0.Trim())
                        {
                            if (str0.Trim() != str1.Trim())
                            {
                                if (str1.Trim() == str2.Trim())
                                {
                                    account.Password = str1;

                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    flag = false;
                                    msg = "Nhập lại mật khẩu không khớp với mật khẩu mới!";
                                }
                            }
                            else
                            {
                                flag = false;
                                msg = "Mật khẩu mới không được trùng với mật khẩu hiện tại!";
                            }
                        }
                        else
                        {
                            flag = false;
                            msg = "Mật khẩu hiển tại không đúng!";
                        }
                    }
                    else
                    {
                        flag = false;
                        msg = "Tài khoản không đúng!";
                    }
                }
                else
                {
                    flag = false;
                    msg = "Nhân viên chưa được tạo tài khoản!";
                }
            }
            else
            {
                flag = false;
                msg = "Nhân viên không tồn tại!";
            }


            return Json(new { msg = msg, flag = flag });
        }

        [Authorize(Roles = "Account_Edit")]
        [HttpPost]
        public async Task<JsonResult> ForgotPassword(int id, string username, string str1,string str2)
        {
            bool flag = true;
            string msg = "Thay đổi thành công!";

            var employee = await _context.Employees.Include(t => t.Accounts).Where(t => t.EmployeeID == id).FirstOrDefaultAsync();

            if (employee != null)
            {
                Account account = employee.Accounts.FirstOrDefault();

                if (account != null)
                {
                    if (account.UserName.Trim() == username.Trim())
                    {
                        if (str1.Trim() == str2.Trim())
                        {
                            account.Password = Common.MD5.CreateHash(str1.Trim());

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            flag = false;
                            msg = "Nhập lại mật khẩu không khớp với mật khẩu mới!";
                        }
                    }
                    else
                    {
                        flag = false;
                        msg = "Tài khoản không đúng!";
                    }
                }
                else
                {
                    flag = false;
                    msg = "Nhân viên chưa được tạo tài khoản!";
                }
            }
            else
            {
                flag = false;
                msg = "Nhân viên không tồn tại!";
            }

            return Json(new { msg = msg, flag = flag });
        }
    }
}
