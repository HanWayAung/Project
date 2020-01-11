using LuckDrawSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuckDrawSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
         //   Session["UserId"] = User.Identity.GetUserId().ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Index(WinningNumber winningNumber, string Random)
        {
            if (Random.Equals("1"))
            {
                var randomNumber = GetRandom();
                winningNumber.LuckyNo = randomNumber;
                try
                {
                    db.WinningNumbers.Add(winningNumber);
                    if (db.SaveChanges() > 0)
                    {

                     //   ViewBag.message = "The last number you  enter is :" + winningNumber.LuckyNo;

                    }
                    else
                    {
                        ViewBag.message = "Something Went wrong!";
                    }
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Random", "Duplicate number or some Wrong ");
                    return View();
                }
            }
            else if (string.IsNullOrEmpty(winningNumber.Byhand))
            {
                ModelState.AddModelError("Byhand", "Enter 4 digit number");
                return View();
            }
            else
            {
                try
                {
                    winningNumber.LuckyNo = Convert.ToInt32(winningNumber.Byhand);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Byhand", "Enter 4 Digit");
                }

                db.WinningNumbers.Add(winningNumber);
                try
                {
                    if (db.SaveChanges() > 0)
                    {


                    }
                    else
                    {
                        ViewBag.message = "Error Occur";
                    }
                }
                catch (Exception ee)
                {
                    ViewBag.error = "validation fail try 4 Digit Number";
                    return View();
                }




                return View();

            }

        }


            private int GetRandom()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }

       

    }
}