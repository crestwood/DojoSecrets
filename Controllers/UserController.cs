using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq;
using DojoSecrets2.Models;

namespace DojoSecrets2.Controllers
{
    public class UserController : Controller
    {
        private YourContext _context;
        public UserController(YourContext context) {
            _context = context;
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register(ViewModel FormData) {
          

            if(ModelState.IsValid) 
            {
                User NewUser = FormData.regUser;
                User emailCheck = _context.Users.SingleOrDefault(u => u.Email == NewUser.Email);
                if(emailCheck == null)
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    _context.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("id", NewUser.Id);
                    return Redirect("/Secrets");
                }
                ModelState.AddModelError("regUser.Email", "Email is already registered.");
                return View("Index", FormData);                
            }
            else {
                ViewBag.errors = ModelState.Values;
                return View("Index",FormData);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(ViewModel FormData) 
        {

            if(ModelState.IsValid)
            {
                LoginCheck LoginUser = FormData.loginUser;
                var user = _context.Users.SingleOrDefault(u => u.Email == LoginUser.Email); 
                if(user !=null)
            {
                    var Hasher = new PasswordHasher<User>();
                    if(0 !=Hasher.VerifyHashedPassword(user, user.Password, LoginUser.Password))
                    {
                        HttpContext.Session.SetInt32("id", user.Id);
                        return Redirect("/Secrets");
                    }
                }
                 ModelState.AddModelError("loginUser.Password", "The email or password provided is incorrect.");
                return View("Index", FormData);
            }
            else
            {
                return View("Index", FormData);
            }
            
        }



            
            [HttpGet]
            [Route("Logout")]
            public IActionResult Logout() {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
           
            public IActionResult Index()
            {
                ViewModel loginReg = new ViewModel()
                {
                    regUser = new User(),
                    loginUser = new LoginCheck()
                };


                return View();
            }









    }
    




}