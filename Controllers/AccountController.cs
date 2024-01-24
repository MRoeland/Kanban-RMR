using Kanban_RMR.Data;
using Kanban_RMR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kanban_RMR.Controllers
{
    [Authorize(Roles = "admin")]
    public class AccountController : Controller
    {
        private readonly KanbanDbContext _context;
        private readonly UserManager<KanbanUser> _userManager;
        private readonly SignInManager<KanbanUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(KanbanDbContext context,
                                    UserManager<KanbanUser> userManager,
                                    SignInManager<KanbanUser> signInManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, /*user.RememberMe*/false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //GET: KanbanUser
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            //var kanbanUsers = _userManager.Users.Include(c => c.Customer).Where(c => c.deleted == false);
            var kanbanusers = _userManager.Users.Include(c => c.Customer);
            return View(await kanbanusers.ToListAsync());
        }

        //GET: KanbanUser
        //[Authorize(Roles = "admin,employee")]
        [AllowAnonymous]
        public async Task<IActionResult> Leaderboard()
        {
            var kanbanusers = _userManager.Users.Where(c => c.deleted == false).OrderByDescending(u => u.Points);
            return View(await kanbanusers.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(string id)
        {
            var kanbanuser = await _userManager.FindByIdAsync(id);
            if (kanbanuser == null)
                return NotFound();

            kanbanuser.Customer = await _context.Customers.FindAsync(kanbanuser.CustomerId);
            return View(kanbanuser);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => r.Name).ToList();
            return View();
        }

        public async Task<bool> isUniqueUserName(string UserName, string Id)
        {
            var isUnique = false;
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null || user.Id == Id)
                isUnique = true;

            return isUnique;
        }
        public async Task<bool> isUniqueEmail(string EmailAddress, string Id)
        {
            var isUnique = false;
            var user = await _userManager.FindByEmailAsync(EmailAddress);
            if (user == null || user.Id == Id)
                isUnique = true;

            return isUnique;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string EmailAddress)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == EmailAddress);
            if (user != null)
                return Json($"Email {EmailAddress} is already in use.");
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUserName(string UserName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == UserName);
            if (user != null)
                return Json($"UserName {UserName} is already in use.");
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateKanbanUserViewModel model)
        {
            var uniqueUsername = await isUniqueUserName(model.UserName, null);
            if (!uniqueUsername)
                ModelState.AddModelError("UserName", "UserName is already in use.");

            var uniqueEmail = await isUniqueEmail(model.EmailAddress, null);
            if (!uniqueEmail)
                ModelState.AddModelError("EmailAddress", "EmailAddress is already in use.");

            if (ModelState.IsValid)
            {
                var kanbanuser = new KanbanUser { UserName = model.UserName, Email = model.EmailAddress };
                kanbanuser.CustomerId = model.CustomerId;
                kanbanuser.Name = model.Name;
                kanbanuser.deleted = false;
                kanbanuser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(kanbanuser, model.Password);
                var resultAddRoles = await _userManager.AddToRolesAsync(kanbanuser, model.Roles);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => r.Name).ToList();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var kanbanuser = await _userManager.FindByIdAsync(id);
            if (kanbanuser == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(kanbanuser);
            var model = new EditKanbanUserViewModel
            {
                Id = kanbanuser.Id,
                UserName = kanbanuser.UserName,
                EmailAddress = kanbanuser.Email,
                Name = kanbanuser.Name,
                CustomerId = kanbanuser.CustomerId,
                EmailConfirmed = kanbanuser.EmailConfirmed,
                deleted = kanbanuser.deleted,
                Roles = userRoles.ToList()
            };
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => r.Name).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditKanbanUserViewModel model)
        {
            var uniqueUsername = await isUniqueUserName(model.UserName, model.Id);
            if (!uniqueUsername)
                ModelState.AddModelError("UserName", "UserName is already in use.");

            var uniqueEmail = await isUniqueEmail(model.EmailAddress, model.Id);
            if (!uniqueEmail)
                ModelState.AddModelError("EmailAddress", "EmailAddress is already in use.");

            if (ModelState.IsValid)
            {
                var kanbanuser = await _userManager.FindByIdAsync(model.Id);
                if (kanbanuser != null)
                {
                    kanbanuser.UserName = model.UserName;
                    kanbanuser.Email = model.EmailAddress;
                    kanbanuser.Name = model.Name;
                    kanbanuser.CustomerId = model.CustomerId;
                    kanbanuser.EmailConfirmed = model.EmailConfirmed;
                    kanbanuser.deleted = model.deleted;

                    //vervang oude roles met nieuwe
                    var oldRoles = await _userManager.GetRolesAsync(kanbanuser);
                    var resultRemoveRoles = await _userManager.RemoveFromRolesAsync(kanbanuser, oldRoles);
                    var resultAddRoles = await _userManager.AddToRolesAsync(kanbanuser, model.Roles);

                    if (resultRemoveRoles.Succeeded && resultAddRoles.Succeeded)
                    {
                        var result = await _userManager.UpdateAsync(kanbanuser);

                        if (result.Succeeded)
                        {
                            if (!User.IsInRole("admin"))
                                return RedirectToAction("Logout");

                            return RedirectToAction("Index");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else
                    {
                        foreach (var error in resultRemoveRoles.Errors.Concat(resultAddRoles.Errors))
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => r.Name).ToList();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var kanbanuser = await _userManager.FindByIdAsync(id);
            if (kanbanuser == null)
                return NotFound();
            var model = new DeleteKanbanUserViewModel { Id = kanbanuser.Id, UserName = kanbanuser.UserName, EmailAddress = kanbanuser.Email };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var kanbanuser = await _userManager.FindByIdAsync(id);
            if (kanbanuser != null)
            {
                //var result = await _userManager.DeleteAsync(kanbanuser);
                kanbanuser.deleted = true;
                var result = await _userManager.UpdateAsync(kanbanuser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

    }
}
