using Microsoft.AspNetCore.Mvc;
using ThinkfulApp.Models;
using ThinkfulApp.Services;

namespace ThinkfulApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.LoggedIn = "You are already logged in. Only log in again if you want to switch users";
            }
            return View("Login");
        }

        public IActionResult ProcessLogin(UserModel user)
        {
            UserModel foundUser = UsersDAO.VerifyAndGetUser(user);
            if (foundUser != null)
            {
                TempData["UserId"] = foundUser.Id;
                ViewBag.User = foundUser;
                ViewBag.Chart = ChartDataDAO.GetChartFromId(foundUser.Id);
                return View("Index");
            }
            else
            {
                ViewBag.FailedLoginAttempt = "That Username and Password could not be found in our records. Please try again.";
                return View("Login");
            }
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult ProcessCreate(UserModel user)
        {
            int userId = UsersDAO.Insert(user);
            if (userId < 1)
            {
                ViewBag.DuplicateUserError = "That Username is already in use. Please use another one.";
                return View("CreateUser");
            }
            else
            {
                TempData["UserId"] = userId;
                user.Id = userId;
                ViewBag.User = user;

                ChartModel chart = ChartDataDAO.GetChartFromId(userId);
                ViewBag.Chart = chart;

                return View("Index");
            }
        }
        public IActionResult Logout()
        {
            if (TempData.ContainsKey("UserId"))
            {
                TempData.Remove("UserId");
                ViewBag.HomeMessage = "You have been successfully logged out.";
                return View("HomeIndex");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
