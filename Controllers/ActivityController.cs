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
    public class ActivityController : Controller
    {

        private ActivityContext _context;
        public ActivityController(ActivityContext context){
            _context = context;
        }

        public IActionResult Delete(int id){
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);

                MyActivity activity = _context.Activities.SingleOrDefault(act => act.ActivityID == id);

                if(activity.User == user){
                    _context.Remove(activity);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Activities", "Home");
        }


        public IActionResult Join(int id){
            // TempData["busy"] = "";
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);

                MyActivity activity = _context.Activities.SingleOrDefault(act => act.ActivityID == id);

                UserActivity userActivity = new UserActivity(){
                    User = user,
                    Activity = activity
                };

                List<UserActivity> otherActivities = _context.UsersActivities
                .Include(uAct => uAct.User)
                .Include(uAct => uAct.Activity)
                .Where(uAct => uAct.User == user)
                .ToList();
                
                bool busy = false;
                foreach(var act in otherActivities){
                    
                    if(act.Activity.ActivityDate == activity.ActivityDate){
                        busy = true;
                    }
                }
                if(busy){
                    // TempData["busy"] = ":Looks like you already have something scheduled for this date";
                    return RedirectToAction("Activities", "Home");
                }

                user.UserActivity.Add(userActivity);
                activity.UserActivity.Add(userActivity);
                _context.Add(userActivity);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Activities", "Home");
        }

        public IActionResult Leave(int id){
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);

                UserActivity userActivity = _context.UsersActivities.SingleOrDefault(uact => uact.UserActivityID == id);

                if(userActivity.User == user){
                    _context.Remove(userActivity);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Activities", "Home");
        }
        
        public IActionResult AddNewActivity(){
            return View();
        }

        public IActionResult CreateActivity(MyActivity activity, string timetype){
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);
                
                if(ModelState.IsValid && activity.ActivityDate > DateTime.Now){
                    activity.User = user;
                    activity.Duration += " " + timetype; 
                    _context.Add(activity);
                    user.myActivity.Add(activity);
                    _context.SaveChanges();
                }
                else{
                    if(activity.ActivityDate < DateTime.Now){
                        ViewBag.error = "Please enter a date in future";
                        return View("AddNewActivity");
                    }
                    return View("AddNewActivity");
                }

            }
            return RedirectToAction("ActivityInfo", new{id = activity.ActivityID});
        }

        public IActionResult ActivityInfo(int id){
            int? UserID = HttpContext.Session.GetInt32("LogedUserID");
            
            if(UserID != null){
                Person user = _context.Users.SingleOrDefault(u => u.UserID == UserID);
                
                MyActivity activity = _context.Activities.Include(act => act.User).SingleOrDefault(act => act.ActivityID == id);
                if(activity == null){
                    return RedirectToAction("Index", "Home");
                }

                List<UserActivity> allUserActivities = _context.UsersActivities
                    .Include(userAct => userAct.User)
                    .Include(userAct => userAct.Activity)
                    .ToList();
                ViewBag.allUserActivities = allUserActivities;
                ViewBag.activity = activity;
                ViewBag.user = user;
                
                return View();
            }
            return RedirectToAction("Index", "Home");
                
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
