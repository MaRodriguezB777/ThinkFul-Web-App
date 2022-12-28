using Microsoft.AspNetCore.Mvc;
using ThinkfulApp.Models;
using ThinkfulApp.Services;

namespace ThinkfulApp.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.User = UsersDAO.GetUserById((int)TempData.Peek("UserId"));
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                return View();
            }
            else
            {
                ViewBag.LoginError = "You cannot view your ThinkFulChart because you are not logged in. Log in to track your progress";
                return View("HomeIndex");
            }
        }
        public IActionResult EditChart()
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                return View();
            }
            else
            {
                ViewBag.HomeMessage = "You cannot update ThinkfulChart because you are not logged in. Log in to track your progress";
                return View("HomeIndex");
            }
        }
        public IActionResult ProcessEdit(ChartModel chart)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                return View("EditChart");
            }
            UserModel user = UsersDAO.GetUserById(chart.Id);
            ViewBag.User = user;
            int foundId = ChartDataDAO.Update(chart);
            if (foundId < 0)
            {
                return View("Error");
            }
            ViewBag.Chart = chart;
            return View("Index");
        }

        public IActionResult AddRemove()
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                return View("AddRemove");
            }
            else
            {
                ViewBag.LoginError = "You cannot view this because you are not logged in";
                return View("HomeIndex");
            }
        }
        public IActionResult DeleteGoal(int ListIndex, string GoalLabel)
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.GoalToBeDeleted = GoalLabel;
                ViewBag.ind = ListIndex;
                ViewBag.Status = ChartDataDAO.DeleteGoal((int)TempData.Peek("UserId"), GoalLabel);
                return PartialView();
            }
            else
            {
                ViewBag.LoginError = "You cannot view this because you are not logged in";
                return View("HomeIndex");
            }
        }

        public IActionResult AddGoal(int NumOfGoals)
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.NumOfGoals = NumOfGoals;
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                return PartialView();
            }
            else
            {
                ViewBag.LoginError = "You cannot view this because you are not logged in";
                return View("HomeIndex");
            }
        }

        //TODO: Fix addingGoals. Currently need to call view from cshtml file and will need to make another view.
        public IActionResult ProcessAddGoal(ChartModel newGoal)
        {
            if (TempData.ContainsKey("UserId"))
            {
                ChartModel chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                ViewBag.Chart = chart;
                if (chart.LabelList.Contains(newGoal.LabelList[0]))
                {
                    ViewBag.AddFailureMessage = "That Goal is already enlisted. Try updating your Progress instead.";
                }
                else if (!ModelState.IsValid)
                {
                    ViewBag.AddFailureMessage = "Could not enlist that Goal. Progress values range from (0-100) and Goals must not be too long";
                }
                else if (ChartDataDAO.AddGoal((int)TempData.Peek("UserId"), newGoal.LabelList[0], (int)newGoal.GoalList[0]) != null)
                {
                    chart.LabelList.Add(newGoal.LabelList[0]);
                    chart.GoalList.Add(newGoal.GoalList[0]);
                    chart.NumOfGoals++;
                    ViewBag.AddSuccessMessage = $"Success! The Goal \"{newGoal.LabelList[0]}\" was added to your ThinkFulChart and with a Progress of {newGoal.GoalList[0]}, you are on the way to mastery! You may add another Goal if you'd like.";
                }
                else
                {
                    return View("Error");
                }
                return View("AddRemove");
            }
            else
            {
                ViewBag.LoginError = "You cannot view this because you are not logged in";
                return View("HomeIndex");
            }
        }
    }
}
