﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Escape.DAL;
using Escape.Models;
using Escape.ViewModels;
using System.Data;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using Escape.Services;

namespace Escape.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly EscapeDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, EscapeDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel loginVM, string returnUrl = null)
        
        {

            AppUser user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("", "Email or Password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password,true,true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Daxil ola  bilmezsiniz!");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View();

            if (_userManager.Users.Any(x => x.UserName == registerVM.UserName))
            {
                ModelState.AddModelError("UserName", "UserName is alredy taken");
                return View();
            }

            if (_userManager.Users.Any(x => x.Email == registerVM.Email))
            {
                ModelState.AddModelError("Email", "Email is alredy taken");
                return View();
            }

            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surename = registerVM.Surename,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Phone = registerVM.Phone,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View();
            }

            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

             //_emailSender.Send(user.Email, "Email Confirme", $"Click <a href=\"{confirmationLink}\">here</a> to verification your email");

            await _userManager.AddToRoleAsync(user, "Member");


            return RedirectToAction(nameof(Login));
        }

     
        //public async Task<IActionResult> ConfirmEmail()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return View("Error");

        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        //}

        //[HttpGet]
        //public IActionResult SuccessRegistration()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }

            AccountProfileViewModel vm = new AccountProfileViewModel
            {
                Profile = new ProfileEditViewModel
                {
                    Name = user.Name,
                    Surename = user.Surename,
                    Email = user.Email,
                    UserName = user.UserName,
                    Phone = user.Phone
                }
            };
            return View(vm);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileEditViewModel profileVM)
        {
            
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = profileVM.Name;
            user.Surename = profileVM.Surename;
            user.Email = profileVM.Email;
            user.Phone = profileVM.Phone;
			user.UserName = profileVM.UserName;
            user.Phone = profileVM.Phone;

            if (!string.IsNullOrEmpty(profileVM.CurrentPassword) || !string.IsNullOrEmpty(profileVM.NewPassword) || !string.IsNullOrEmpty(profileVM.ConfirmPassword))
            {
                

                if (string.IsNullOrEmpty(profileVM.NewPassword)&& !string.IsNullOrEmpty(profileVM.CurrentPassword))
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("NewPassword", "Please enter your New Password");
                    return View(vm);

                }
                else if (!string.IsNullOrEmpty(profileVM.NewPassword) && string.IsNullOrEmpty(profileVM.CurrentPassword))
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("CurrentPassword", "Please enter your Current Password");
                    return View(vm);

                }
                else if (profileVM.NewPassword.Length<8)
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("NewPassword", "New Password length is must be longer than 8");
                    return View(vm);
                }
                else 
                {
                    if (!await _userManager.CheckPasswordAsync(user,profileVM.CurrentPassword))
                    {
                        AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                        ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct!!!");
                        return View(vm);
                    }
                 var newPass = await _userManager.ChangePasswordAsync(user, profileVM.CurrentPassword, profileVM.NewPassword);
                    if (!newPass.Succeeded)
                    {
                        foreach (var err in newPass.Errors)
                            ModelState.AddModelError("", err.Description);
                        return View();
                    }
                }

            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("profile");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel passwordVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(passwordVM.Email);

            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("Email", "Email is not correct");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string url = Url.Action("resetpassword", "account", new { email = passwordVM.Email, token = token }, Request.Scheme);

            _emailSender.Send(passwordVM.Email, "Reset Password", $"Click <a href=\"{url}\">here</a> to reset your password");

            return RedirectToAction("login");


        }

        public async Task<IActionResult> Resetpassword(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.IsAdmin || !await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                return RedirectToAction("login");

            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resetpassword(ResetPasswordViewModel resetVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(resetVM.Email);

            if (user == null || user.IsAdmin)
                return RedirectToAction("login");

            var result = await _userManager.ResetPasswordAsync(user, resetVM.Token, resetVM.Password);

            if (!result.Succeeded)
            {
                return RedirectToAction("login");
            }

            return RedirectToAction("login");
        }

        public IActionResult GoogleLogin()
        {
            string url = Url.Action("googleresponse", "account", Request.Scheme);

            var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", url);

            return new ChallengeResult("Google", prop);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var info = _signInManager?.GetExternalLoginInfoAsync().Result;

            if (info == null) return RedirectToAction("login");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            AppUser user = _userManager.FindByEmailAsync(email).Result;

            if (user == null)
            {
                user = new AppUser { Email = email, UserName = email };
                var result = _userManager.CreateAsync(user).Result;

                if (!result.Succeeded) return RedirectToAction("login");

                await _userManager.AddToRoleAsync(user, "Member");
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("index", "home");
        }

    }
}
