using Inventory_Management_System.Models;
using Inventory_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Inventory_Management_System.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class UsersController : Controller
    {
        public IMSDatabaseContext _context { get; set; }
        public UserManager<IdentityUser> _usermanager { get; set; }
        public UsersController(IMSDatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: UsersController
        public async Task<IActionResult> Index()
        {
            var userViewModels = from u in _context.UserRoles
                                 join c in _context.Users on u.UserId equals c.Id
                                 join r in _context.Roles on u.RoleId equals r.Id
                                 select new UserViewModel
                                 {
                                     UserId = c.Id,
                                     UserName = c.UserName,
                                     Role = r.Name
                                 };
            return userViewModels != null ? 
                View(await userViewModels.ToListAsync()) : NotFound();
        }

        // GET: UsersController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UsersController/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            try
            {
                var user = new IdentityUser { UserName =  userViewModel.UserName, Email = userViewModel.UserName };
                await _usermanager.CreateAsync(user);
                await _usermanager.AddToRoleAsync(user, userViewModel.Role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
                return View(userViewModel);
            }
        }

        // GET: UsersController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = await (from u in _context.UserRoles
                                 join c in _context.Users on u.UserId equals c.Id
                                 join r in _context.Roles on u.RoleId equals r.Id
                                 where c.Id == id
                                 select new UserViewModel
                                 {
                                     UserId = c.Id,
                                     UserName = c.UserName,
                                     Role = r.Name
                                 }).FirstOrDefaultAsync();
            
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
            return View(userViewModel);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel userViewModel)
        {
            if (id != userViewModel.UserId)
            {
                return NotFound();
            }
            try
            {
                Collection<string> rolesToRemove = new Collection<string> { "User", "Stock manager", "Administrator" };
                var user = await _usermanager.FindByIdAsync(id);
                if (user == null) { return NotFound(); }
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.UserName;
                foreach (var role in rolesToRemove)
                {
                    await _usermanager.RemoveFromRoleAsync(user, role);
                }
                await _usermanager.AddToRoleAsync(user, userViewModel.Role);
                await _usermanager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
                return View(userViewModel);
            }
        }

        // GET: UsersController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'Users' is null.");
            }
            try
            {
                var user = await _usermanager.FindByIdAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }
                else
                {
                    return NotFound();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
