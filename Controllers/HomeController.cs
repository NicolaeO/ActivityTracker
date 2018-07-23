using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityTracker.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Controllers
{
    public class HomeController : Controller
    {

        private ActivityContext _context;
        public HomeController(ActivityContext context){
            _context = context;
        }


        public IActionResult Index(){
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                return RedirectToAction("Activities");
            }
            
            return View();
        }

        public IActionResult Activities()
        {
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID == null){
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
           
            Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);
            ViewBag.user = user; 

            List<MyActivity> activities = _context.Activities
                .Include(act => act.User)
                .Where(act => act.ActivityDate>DateTime.Now)
                .OrderByDescending(msg => msg.CreatedAt)
                .ToList();
            ViewBag.allActivities = activities;

            List<UserActivity> userActivity = _context.UsersActivities
            .Include(uact => uact.User)
            .Include(uact => uact.Activity)
            .ToList();
            ViewBag.userActivity = userActivity;

            return View();
        }


public IActionResult Create(Person person, string ConfirmPass){

            ViewBag.error ="";

            if(ModelState.IsValid && person.Password == ConfirmPass){
                PasswordHasher<Person> Hasher = new PasswordHasher<Person>();
                person.Password = Hasher.HashPassword(person, person.Password);
                //Save your user object to the database
                _context.Add(person);
                _context.SaveChanges();

                Person ReturnedValue = _context.Users.SingleOrDefault(u => u.Email == person.Email); 

                HttpContext.Session.SetInt32("LogedUserID", ReturnedValue.UserID);
                
                return RedirectToAction("Activities");
            }
            else{
                if(person.Password != ConfirmPass){
                    ViewBag.error = "Password did not match";
                }
                return View("Index");
            }
        }



       public IActionResult LogOff(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult LogIn(string Email, string Password){
            ViewBag.error ="";

            Person dbUser = _context.Users.SingleOrDefault(u => u.Email == Email);           
            if(dbUser != null){
                var Hasher = new PasswordHasher<Person>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(dbUser, dbUser.Password, Password))
                {
                    //Handle success
                    HttpContext.Session.SetInt32("LogedUserID", dbUser.UserID);
                    return RedirectToAction("Activities");
                }
                else{
                    ViewBag.error = "Please check the password or email";
                    return View("Index");
                }
            }
            else{
                ViewBag.error = "Please check the password or email";
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
