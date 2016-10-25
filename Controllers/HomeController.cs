using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using theWall.Models;
using theWall.Factory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace theWall.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserFactory userFactory;

        public HomeController()
        {
            //Instantiate a UserFactory object that is immutable (READONLY)
            //This is establish the initial DB connection for us.
            userFactory = new UserFactory();
        }

        
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Errors = "";
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult login(string email, string password)
        {
            if(userFactory.FindByEmail(email) != null)
            {
                User CheckUser = userFactory.FindByEmail(email);
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                if(Hasher.VerifyHashedPassword(CheckUser, CheckUser.password, password) == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("user", CheckUser.email);
                    return RedirectToAction("wall");
                }
            }
            ViewBag.errors = "Incorrect Email or Password.";
            return View("Index");
        }

        [HttpGet]
        [Route("/register")]
        public IActionResult register()
        {
            ViewBag.Errors = ModelState.Values;            
            return View("register");
        }

        [HttpPost]
        [Route("/createuser")]
        public IActionResult createuser(User NewUser)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.password = Hasher.HashPassword(NewUser, NewUser.password);
                userFactory.Add(NewUser);
                return RedirectToAction("wall");
            }
            ViewBag.Errors = ModelState.Values;
            return View("register");
        }

        [HttpGet]
        [Route("/wall")]
        public IActionResult wall()
        {
            User user = userFactory.FindByEmail(HttpContext.Session.GetString("user"));
            HttpContext.Session.SetInt32("id", user.id);
            ViewBag.messages = userFactory.FindMessages();
            ViewBag.comments = userFactory.FindComments();
            ViewBag.user = user.first_name;
            ViewBag.errors = ModelState.Values;
            return View("wall");
        }

        [HttpPost]
        [Route("/postmessage")]
        public IActionResult postmessage(Message newmessage)
        {
            if(ModelState.IsValid)
            {
                userFactory.AddMessage( (int)HttpContext.Session.GetInt32("id"), newmessage.message);
                return RedirectToAction("wall");
            }
            ViewBag.errors = ModelState.Values;
            return View("wall");
        }

        [HttpPost]
        [Route("/postcomment/{id}")]
        public IActionResult postcomment(int id, string comment)
        {
            if(comment.Length > 3)
            {
                userFactory.AddComment((int)HttpContext.Session.GetInt32("id"), id, comment);
            }
            return RedirectToAction("wall");
        }

        [HttpGet]
        [Route("/logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
