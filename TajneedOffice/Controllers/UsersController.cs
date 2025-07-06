using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using System.Security.Claims;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Controller for user management operations
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TajneedOfficeDbContext _context;

        public UsersController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            TajneedOfficeDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string searchTerm, string? role, bool? isActive)
        {
            ViewBag.Roles = await GetRolesSelectList();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedRole = role;
            ViewBag.SelectedIsActive = isActive;

            var users = await GetFilteredUsers(searchTerm, role, isActive);
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rank = await _context.Ranks.FindAsync(user.RankId);

            ViewBag.UserRoles = userRoles;
            ViewBag.Rank = rank;

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Ranks = await GetRanksSelectList();
            ViewBag.Roles = await GetRolesSelectList();
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,ServiceNumber,RankId,Position,IsActive")] User user, string password, string confirmPassword, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("password", "كلمة المرور مطلوبة");
                }
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "كلمة المرور غير متطابقة");
                }
                if (string.IsNullOrEmpty(role))
                {
                    ModelState.AddModelError("role", "الدور مطلوب");
                }

                if (ModelState.IsValid)
                {
                    // Check if service number already exists
                    var existingUser = await _userManager.FindByNameAsync(user.ServiceNumber);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("ServiceNumber", "الرقم العسكري مستخدم بالفعل");
                        ViewBag.Ranks = await GetRanksSelectList();
                        ViewBag.Roles = await GetRolesSelectList();
                        return View(user);
                    }

                    // Set username to service number
                    user.UserName = user.ServiceNumber;

                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        // Assign role
                        if (!string.IsNullOrEmpty(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }

                        TempData["SuccessMessage"] = "تم إنشاء المستخدم بنجاح";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ غير متوقع: " + ex.Message;
            }

            ViewBag.Ranks = await GetRanksSelectList();
            ViewBag.Roles = await GetRolesSelectList();
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Ranks = await GetRanksSelectList();
            ViewBag.Roles = await GetRolesSelectList();
            ViewBag.CurrentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,ServiceNumber,RankId,Position,IsActive")] User user, string role)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Check if service number is changed and already exists
                if (existingUser.ServiceNumber != user.ServiceNumber)
                {
                    var userWithSameServiceNumber = await _userManager.FindByNameAsync(user.ServiceNumber);
                    if (userWithSameServiceNumber != null)
                    {
                        ModelState.AddModelError("ServiceNumber", "الرقم العسكري مستخدم بالفعل");
                        ViewBag.Ranks = await GetRanksSelectList();
                        ViewBag.Roles = await GetRolesSelectList();
                        ViewBag.CurrentRole = (await _userManager.GetRolesAsync(existingUser)).FirstOrDefault();
                        return View(user);
                    }
                }

                // Update user properties
                existingUser.FullName = user.FullName;
                existingUser.ServiceNumber = user.ServiceNumber;
                existingUser.UserName = user.ServiceNumber;
                existingUser.RankId = user.RankId;
                existingUser.Position = user.Position;
                existingUser.IsActive = user.IsActive;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    // Update role
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    if (currentRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    }

                    if (!string.IsNullOrEmpty(role))
                    {
                        await _userManager.AddToRoleAsync(existingUser, role);
                    }

                    TempData["SuccessMessage"] = "تم تحديث المستخدم بنجاح";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.Ranks = await GetRanksSelectList();
            ViewBag.Roles = await GetRolesSelectList();
            ViewBag.CurrentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return View(user);
        }

        // GET: Users/ChangePassword/5
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserName = user.FullName;
            ViewBag.ServiceNumber = user.ServiceNumber;

            return View();
        }

        // POST: Users/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string newPassword)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "كلمة المرور مطلوبة");
                ViewBag.UserName = user.FullName;
                ViewBag.ServiceNumber = user.ServiceNumber;
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تغيير كلمة المرور بنجاح";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.UserName = user.FullName;
            ViewBag.ServiceNumber = user.ServiceNumber;
            return View();
        }

        // POST: Users/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var status = user.IsActive ? "تفعيل" : "تعطيل";
                TempData["SuccessMessage"] = $"تم {status} المستخدم بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تغيير حالة المستخدم";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user can be deleted
            // Future: Check for any related data

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم حذف المستخدم بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف المستخدم";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper methods
        private async Task<List<User>> GetFilteredUsers(string searchTerm, string? role, bool? isActive)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => 
                    u.FullName.Contains(searchTerm) || 
                    u.ServiceNumber.Contains(searchTerm));
            }

            if (isActive.HasValue)
            {
                users = users.Where(u => u.IsActive == isActive.Value);
            }

            var userList = users.ToList();

            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                userList = userList.Where(u => usersInRole.Any(ur => ur.Id == u.Id)).ToList();
            }

            return userList;
        }

        private async Task<SelectList> GetRanksSelectList()
        {
            var ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            return new SelectList(ranks, "RankId", "RankName");
        }

        private async Task<SelectList> GetRolesSelectList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new SelectList(roles, "Name", "Name");
        }
    }
} 