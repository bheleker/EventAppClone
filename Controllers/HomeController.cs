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
using Newtonsoft.Json;
using System.Net.Http; 


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
                if(dbContext.Users.Where(u => u.Email == NewUser.Users.Email).First().Password == null)
                {
                    ModelState.AddModelError("Users.Email", "Account already exists. Please sign in on Google.");
                }
                else{
                    ModelState.AddModelError("Users.Email", "Email is already in use.");
                }
                    return View("Index");
                }
                if(NewUser.Users.Password == null){
                    ModelState.AddModelError("Users.Password", "Password must not be empty.");
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
        [HttpGet("google/verify")]
        public void GoogleVerify()
        {
            Response.Redirect("https://accounts.google.com/o/oauth2/v2/auth?client_id=868360967766-16m02f3h7j62d42jhbl3pkpmia124740.apps.googleusercontent.com&response_type=code&scope=openid%20email%20profile&redirect_uri=https://ec2-3-136-161-174.us-east-2.compute.amazonaws.com//GoogleSignIn&state=abcdef");
        }
        [HttpGet("GoogleSignIn")]
        public async Task<ActionResult> GoogleSignIn(string code, string state, string session_state)
        {
            if(code != null){

                var httpClient = new HttpClient  
            {  
                BaseAddress = new Uri("https://www.googleapis.com")  
            };  
            var requestUrl = $"oauth2/v4/token?code={code}&client_id=868360967766-16m02f3h7j62d42jhbl3pkpmia124740.apps.googleusercontent.com&client_secret=FHMvE9-Hc6ZOh9H0Af3oCiLQ&redirect_uri=https://ec2-3-136-161-174.us-east-2.compute.amazonaws.com//GoogleSignIn&grant_type=authorization_code";  

            var dict = new Dictionary<string, string>  
            {  
                { "Content-Type", "application/x-www-form-urlencoded" }  
            };  
            var req = new HttpRequestMessage(HttpMethod.Post, requestUrl) { Content = new FormUrlEncodedContent(dict) };  
            var response = await httpClient.SendAsync(req);  
            GmailToken token = JsonConvert.DeserializeObject<GmailToken>(await response.Content.ReadAsStringAsync());
                var httpClientTwo = new HttpClient  
            {  
                BaseAddress = new Uri("https://www.googleapis.com")  
            };  
            string url = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={token.AccessToken}";  
            var responseTwo = await httpClient.GetAsync(url);  
            UserProfile GoogleUserInfo = JsonConvert.DeserializeObject<UserProfile>(await responseTwo.Content.ReadAsStringAsync()); 
            if(dbContext.Users.Any(u => u.Email == GoogleUserInfo.Email))
                {
                    User GoogleUser = dbContext.Users.Where(u => u.Email == GoogleUserInfo.Email).First();
                    HttpContext.Session.SetInt32("UserId", GoogleUser.UserId);
                    return RedirectToAction("Dashboard");
                }
                else{
                    User user = new User{Name=GoogleUserInfo.Name, Email = GoogleUserInfo.Email};
                    dbContext.Add(user);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else{
                return RedirectToAction("Index");
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
            IEnumerable<Message> retMessageList = dbContext.Messages
            .Include(m => m.Creator)
            .Include(m => m.SpecificActivity)
            .Where(m => m.ActivityId == id)
            .OrderBy(m => m.CreatedAt)
            .ToList();
            User retUser = dbContext.Users.FirstOrDefault(u=>u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            
            ActivityViewModel viewModel = new ActivityViewModel()
            {
                viewActivityList = ThisActivity,
                viewActivityModel = retActivity,
                viewSessionId = (int)HttpContext.Session.GetInt32("UserId"),
                viewMessageList = retMessageList,
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
