using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Login()
        {
            return View();
        }
        public ActionResult Index()
        {
            var allUsers= _userProfileRepository.GetAll();
            return View(allUsers);
        }

        public IActionResult Details(int id)
        {

            UserProfile user = _userProfileRepository.GetById(id);
            return View(user);
        }
        public ActionResult DeactivateUser(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);

            userProfile.IsActive = false;
            _userProfileRepository.ChangeUser(userProfile);

            return RedirectToAction("Index");


        }
        public ActionResult ReactivateUser(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);

            userProfile.IsActive = true;
            _userProfileRepository.ChangeUser(userProfile);

            return RedirectToAction("Index");


        }
        public ActionResult MakeAdmin(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
            userProfile.UserTypeId = 1;
            _userProfileRepository.ChangeUser(userProfile);
            return RedirectToAction("Index");
        }

        public ActionResult RevokeAdmin(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetById(id);
            userProfile.UserTypeId = 2;
            _userProfileRepository.ChangeUser(userProfile);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }
            else if (userProfile.IsActive != true)
            {
                ModelState.AddModelError("IsActive", "Account Disabled");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.UserType.Name)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            RegisterViewModel vm = new RegisterViewModel()
            {
                UserProfile = new UserProfile(),
               
            };

            return View(vm);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
        {
            registerViewModel.UserProfile.UserTypeId = 2;
            registerViewModel.UserProfile.IsActive = true;


            var userProfile = _userProfileRepository.GetByEmail(registerViewModel.UserProfile.Email);

            if (userProfile.Email == registerViewModel.UserProfile.Email)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View("Create");
            }

            try
            {

                _userProfileRepository.AddUser(registerViewModel.UserProfile);
               await Login(new Credentials
                {
                    Email = registerViewModel.UserProfile.Email
                }
                );
               
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(registerViewModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
