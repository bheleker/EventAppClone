using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EventApp.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }
                [HttpPost]
        public IActionResult Register(LogUser NewUser)
        {   
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == NewUser.Users.Email))
                {
                    ModelState.AddModelError("Email", "Email's already in use.");
                    return View("Index");
                }
                PasswordHasher<User> PassHash = new PasswordHasher<User>();
                NewUser.Users.Password = PassHash.HashPassword(NewUser.Users, NewUser.Users.Password);
                User UserSave = NewUser.Users;
                dbContext.Add(UserSave);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", UserSave.UserId);
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Index");
            }
        }
                [HttpPost]
        public IActionResult LogIn(LogUser LogUser)
        {
            var PassHash = new PasswordHasher<LogInUser>();
            User CurrentLog = dbContext.Users.Where(use => use.Email == LogUser.Logs.Email).FirstOrDefault();
            if(CurrentLog == null)
            {
                ModelState.AddModelError("Logs.Email", "*Invalid Email or Password");
                return View("Index");
            }
            var result = PassHash.VerifyHashedPassword(LogUser.Logs, CurrentLog.Password, LogUser.Logs.Password);
            if(result == 0)
            {
                ModelState.AddModelError("Logs.Email", "Invalid Email or Password");
                return View("Index");
            }
            else{
                HttpContext.Session.SetInt32("UserId", CurrentLog.UserId);
                return RedirectToAction("Dashboard");
            }

        }
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                IEnumerable<ActivityModel> Activities = dbContext.Activities.Where(act => act.Date > DateTime.Now)
                .OrderBy(act => act.Date)
                .Include(act => act.Joins);
                ViewBag.CurrentUser = (int)HttpContext.Session.GetInt32("UserId");
                User theUser = dbContext.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"))
                .FirstOrDefault();
                ViewBag.Name = theUser.Name;
                foreach(ActivityModel a in Activities)
                {
                    a.Creator = dbContext.Users.Where(u => u.UserId == a.UserId).First();
                }
            return View(Activities);
            }
            else{
                return RedirectToAction("Index");
            }
        }
        public IActionResult NewActivity()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
            return View();
            }
            else{
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult CreateActivity(ActivityModel NewActivity)
        {
            if(NewActivity.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", "Please select a valid date.");
                }
            if(ModelState.IsValid){
                NewActivity.UserId = (int)HttpContext.Session.GetInt32("UserId");
                dbContext.Add(NewActivity);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                return View("NewActivity");
            }
        }
        [HttpGet("Join/{id:int}")]
        public IActionResult Join(int id)
        {
            Join ThisJoin = new Join{ActivityId = id, UserId = (int)HttpContext.Session.GetInt32("UserId")};
            dbContext.Add(ThisJoin);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("Leave/{id:int}")]
        public IActionResult Leave(int id)
        {
            Join ThisJoin = dbContext.Joins.Where(j => j.ActivityId == id && j.UserId == (int)HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
            dbContext.Remove(ThisJoin);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("ActivityDelete/{id:int}")]
        public IActionResult ActivityDelete(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") != null){
            ActivityModel Activity = dbContext.Activities.Where(a => a.ActivityId == id).First();
            if((int)HttpContext.Session.GetInt32("UserId") == Activity.UserId)
            {
                dbContext.Remove(Activity);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                return RedirectToAction("Dashboard");
            }
            }
            else{
                return RedirectToAction("Index");
            }
        }
        [HttpGet("Activity/{id:int}")]
        public IActionResult Activities(int id)
        {
            if(HttpContext.Session.GetInt32("UserId") != null){
            IEnumerable<ActivityModel> ThisActivity = dbContext.Activities.Where(a => a.ActivityId == id)
            .Include(a => a.Joins).ThenInclude(j => j.User);
            ActivityModel retActivity = dbContext.Activities.FirstOrDefault(a => a.ActivityId == id);
            IEnumerable<Message> retActivityList = dbContext.Messages
            .Include(m => m.SpecificActivity)
            .Include(m => m.Creator)
            .Where(m => m.ActivityId == id)
            .ToList();
            User retUser = dbContext.Users.FirstOrDefault(u=>u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            
            ActivityViewModel viewModel = new ActivityViewModel()
            {
                viewActivityList = ThisActivity,
                viewActivityModel = retActivity,
                viewSessionId = (int)HttpContext.Session.GetInt32("UserId"),
                viewMessageList = retActivityList,
                viewSessionUserName = retUser.Name,

            };
            foreach(ActivityModel a in ThisActivity){
            
            a.Creator = dbContext.Users.Where(u => u.UserId == a.UserId).First();
            }
            if(ThisActivity == null){
                return RedirectToAction("Dashboard");
            }
            return View(viewModel);
            }
            return RedirectToAction("Index");
        }
        // [HttpPost]
        // public IActionResult PostMessage(ActivityViewModel viewModel, int id)
        // {
        //     System.Console.WriteLine("########################################################");
        //     viewModel.viewMessage.UserId = (int)HttpContext.Session.GetInt32("UserId");
        //     viewModel.viewMessage.ActivityId = id;
        //     dbContext.Messages.Add(viewModel.viewMessage);
        //     dbContext.SaveChanges();
        //     return Redirect($"/Activity/{id}");
        // }

        [HttpPost("/postmessage")]
        public IActionResult AjaxPostMessage(AjaxMessageViewModel viewModel)
        {
            Message postMessage = new Message()
            {
                MessageBody = viewModel.message,
                UserId = (int)HttpContext.Session.GetInt32("UserId"),
                ActivityId = viewModel.activityId,
            };
            dbContext.Messages.Add(postMessage);
            dbContext.SaveChanges();
            return new EmptyResult();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
