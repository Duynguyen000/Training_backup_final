using Training.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Training.Controllers
{
    public class UserController : Controller
    {
        protected ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        // TODO: add role info
        // GET: Index
        public ActionResult Index()
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            var users = _context.Users.ToList();
            foreach (var item in users)
            {
                UserViewModel userView = new UserViewModel
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    Email = item.Email
                };

                userViewModels.Add(userView);
            }
            return View(userViewModels);
        }

        public ActionResult GetInstructor()
        {
            var roles = _context.Roles.Where(r => r.Name == "instructor");

            var instructorList = (from user in _context.Users
                                  where user.Roles.Any(r => r.RoleId == roles.FirstOrDefault().Id)
                                  select user).ToList();

            return PartialView("_InstructorSelectPartial", instructorList);
        }

        //Create action
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: User/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.role = _context.Roles.Select(r => r.Name).ToList();
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FullName = userViewModel.FullName
                };
                var userCreateResult = await UserManager.CreateAsync(user, userViewModel.Password);
                var assignRoleResult = await UserManager.AddToRoleAsync(user.Id, userViewModel.RoleName);

                if (userCreateResult.Succeeded &&
                    assignRoleResult.Succeeded)
                    return RedirectToAction("Index");

                AddErrors(userCreateResult);
                AddErrors(assignRoleResult);
            }

            // If we got this far, something failed, redisplay form
            if (!ModelState.IsValid) return RedirectToAction("Create", "User");
            return View(userViewModel);
        }

        //edit action
        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindById(Id);
            var role = await UserManager.GetRolesAsync(Id);
            EditViewModel user = new EditViewModel();
            user.FullName = appUser.FullName;
            user.Email = appUser.Email;
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string Id, EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindById(Id);
            currentUser.FullName = model.FullName;
            currentUser.Email = model.Email;
            if (model.Password != null)
            {
                currentUser.PasswordHash = manager.PasswordHasher.HashPassword(model.Password);
            }
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await manager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesForUser = await manager.GetRolesAsync(id);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await manager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    await manager.DeleteAsync(user);
                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}