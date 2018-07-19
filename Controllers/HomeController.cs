using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DojoSecrets2.Models;

namespace DojoSecrets2.Controllers
{
    public class HomeController : Controller
    {
        private YourContext _context;
        public HomeController(YourContext context) {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Secrets")]
            public IActionResult Secrets()
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == HttpContext.Session.GetInt32("id"));           
            ViewBag.CurrentUser = CurrentUser;    
            List<Secret> allSecrets =_context.Secrets
                                        .Include(sec => sec.Likers)
                                            .ThenInclude(like => like.Liker)
                                        .ToList();

            ViewBag.allSecrets = allSecrets.OrderByDescending(sec => sec.Created_At).Take(10);

            ViewBag.CurrentTime = DateTime.Now;        
            return View();
        }

        [HttpGet]
        [Route("Popular")]
            public IActionResult Popular()
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == HttpContext.Session.GetInt32("id"));           
            ViewBag.CurrentUser = CurrentUser;    
            List<Secret> allSecrets =_context.Secrets
                                        .Include(sec => sec.Likers)
                                            .ThenInclude(like => like.Liker)
                                        .ToList();

            ViewBag.allSecrets = allSecrets.OrderByDescending(sec => sec.Likers.Count);

            ViewBag.CurrentTime = DateTime.Now;
           

            return View();
        }

        [HttpPost]
        [Route("addSecret")]
        public IActionResult addSecret(Secret secret)
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == HttpContext.Session.GetInt32("id"));           
            ViewBag.User = CurrentUser;
            if(ModelState.IsValid)
            {
                secret.Creator = CurrentUser;
                _context.Add(secret);
                _context.SaveChanges();
                return Redirect("Secrets");
            }
            ModelState.AddModelError("Content", "Please enter between 8 and 255 characters");
            return Redirect("Secrets");


            
        }
        [HttpGet]
        [Route("Delete/{secret_id}")]
        public IActionResult Delete(int secret_id)
        {
            
            Secret CurrentSecret = _context.Secrets.SingleOrDefault(sec => sec.Id == secret_id);         
            _context.Remove(CurrentSecret);
            _context.SaveChanges();
        
            return RedirectToAction("Secrets");
        }



        [HttpGet]
        [Route("Like/{secret_id}")]
        public IActionResult Like(int secret_id)
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == user_id);
            

            Secret CurrentSecret = _context.Secrets.SingleOrDefault(sec => sec.Id == secret_id); 
            
            Like liked = new Like();
            liked.Liker =CurrentUser;
            liked.UserId = CurrentUser.Id;
            liked.SecretId = secret_id;
            liked.Secret = CurrentSecret;

            _context.Add(liked);
            _context.SaveChanges();
        
            return RedirectToAction("Secrets");
        }
        [HttpGet]
        [Route("Like2/{secret_id}")]
        public IActionResult Like2(int secret_id)
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User CurrentUser = _context.Users.SingleOrDefault(user => user.Id == user_id);
            

            Secret CurrentSecret = _context.Secrets.SingleOrDefault(sec => sec.Id == secret_id); 
            
            Like liked = new Like();
            liked.Liker =CurrentUser;
            liked.UserId = CurrentUser.Id;
            liked.SecretId = secret_id;
            liked.Secret = CurrentSecret;

            _context.Add(liked);
            _context.SaveChanges();
        
            return RedirectToAction("Popular");
        }
        [HttpGet]
        [Route("Delete2/{secret_id}")]
        public IActionResult Delete2(int secret_id)
        {
            
            Secret CurrentSecret = _context.Secrets.SingleOrDefault(sec => sec.Id == secret_id);         
            _context.Remove(CurrentSecret);
            _context.SaveChanges();
        
            return RedirectToAction("Popular");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
         
    }
}
