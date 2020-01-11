using LuckDrawSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuckDrawSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Random r = new Random();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
          
        }

        [HttpPost]
        public ActionResult Index(PrizeWinner prize, string isRandom, string Number)
        {
            if (prize.PrizeType == null)
            {
                //ModelState.AddModelError("message", "Please select prize type bro");
                return View();
            }
            if (prize.PrizeType.Equals("1"))
            {
                if (GetGrandPirze() != 0)
                {
                    int firstwinner = GetGrandPirze();
                    var user = db.WinningNumbers.Single(x => x.LuckyNo == firstwinner);
                    PrizeWinner winner = new PrizeWinner() { PrizeType = "1", UserId = user.UserId.ToString(), WinningNumber = GetGrandPirze().ToString() };
                    var prizeDetail = db.PrizeWinners.Where(x => x.PrizeType.Equals(prize.PrizeType)).FirstOrDefault();
                    if (prizeDetail == null)
                    {
                        if (AddToDatabase(winner) == true)
                        {
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("message", "Something Went Wrong");


                    }
                    else
                    {
                        ModelState.AddModelError("PrizeType", "The prize is already draw");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("message", "There is no user to Draw");
                }
            }
            else if (isRandom.Equals("0"))
            {
                if (string.IsNullOrEmpty(Number))
                {
                    ModelState.AddModelError("Number", "You have to enter 4 digit number");
                    return View();
                }
                else
                {
                    try
                    {
                        var number = Convert.ToInt32(Number);
                        var user = db.WinningNumbers.Where(x => x.LuckyNo == number).FirstOrDefault();
                        if (user == null)
                        {
                            ModelState.AddModelError("Number", "The number did't not match!");
                            return View();
                        }
                        else
                        {
                            PrizeWinner prize1 = new PrizeWinner() { PrizeType = prize.PrizeType, UserId = user.UserId, WinningNumber = number.ToString() };
                            var prizeDetail = db.PrizeWinners.Where(x => x.PrizeType.Equals(prize.PrizeType)).FirstOrDefault();
                            if (prizeDetail == null)
                            {
                                if (AddToDatabase(prize1) == true)
                                {
                                    return RedirectToAction("Index");
                                }
                                ModelState.AddModelError("message", "Something Went Wrong");


                            }
                            else
                            {
                                ModelState.AddModelError("PrizeType", "The prize is already draw");
                                return View();
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Number", "Be Serious! 4 Digit Number Bro");
                        return View();
                    }

                }

            }
            else
            {
                if (getRandomlyOther() == 0)
                {
                    ModelState.AddModelError("PirzeType", "Please select grand prize first or there is no winningnumber reamin! ");
                    return View();
                }
                var winnerNumber = getRandomlyOther();
                var user = db.WinningNumbers.Where(x => x.LuckyNo == winnerNumber).FirstOrDefault();
                PrizeWinner prize1 = new PrizeWinner { PrizeType = prize.PrizeType, WinningNumber = winnerNumber.ToString(), UserId = user.UserId };
                var prizeDetail = db.PrizeWinners.Where(x => x.PrizeType.Equals(prize.PrizeType)).FirstOrDefault();
                if (prizeDetail == null)
                {
                    if (AddToDatabase(prize1) == true)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("message", "Something Went Wrong");

                }
                else
                {
                    ModelState.AddModelError("PrizeType", "The prize is already draw");
                    return View();
                }


            }
            return View();
        }

        private bool AddToDatabase(PrizeWinner prize)
        {
            db.PrizeWinners.Add(prize);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private int GetGrandPirze()
        {
            //Random r = new Random();
            var lists = new List<int>();
            var grandPrizeWinners = db.WinningNumbers.GroupBy(x => x.UserId).OrderByDescending(x => x.Count()).Take(2);
            if (grandPrizeWinners != null)
            {
                foreach (var group in grandPrizeWinners)
                {
                    foreach (var data in group)
                    {
                        lists.Add(data.LuckyNo);
                    }
                }

                var randomList = lists.OrderBy(x => r.Next(lists.Count));
                var index = r.Next(randomList.ToList().Count);
                return randomList.ToList()[index];
            }
            return 0;
        }

        private int getRandomlyOther()
        {
            List<int> getAllWinningNumbers = new List<int>();
            List<int> getWinnerNumbersList = new List<int>();
            List<SelectListItem> remainingNumber = new List<SelectListItem>();
            foreach (var winningNumber in db.WinningNumbers.ToList())
            {
                getAllWinningNumbers.Add(winningNumber.LuckyNo);
            }

            var winnerList = db.PrizeWinners.ToList();
            if (winnerList == null)
            {
                return 0;
            }
            else
            {
                foreach (var winner in winnerList)
                {
                    var winnerWinningNumbers = db.WinningNumbers.Where(x => x.UserId.Equals(winner.UserId)).ToList();
                    foreach (var winnerWinningNumber in winnerWinningNumbers)
                    {
                        getWinnerNumbersList.Add(winnerWinningNumber.LuckyNo);
                    }
                }
            }

            foreach (var item in getWinnerNumbersList)
            {
                getAllWinningNumbers.Remove(item);
            }

            foreach (var item in getAllWinningNumbers)
            {
                remainingNumber.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() });
            }

            ViewBag.remainingNumber = remainingNumber;



            return getPrize(getAllWinningNumbers);
        }

        private int getPrize(List<int> getAllWinningNumbers)
        {

            if (getAllWinningNumbers.Count > 0)
            {
                var numbers = getAllWinningNumbers.OrderBy(x => r.Next(getAllWinningNumbers.Count)).ToList();
                var index = r.Next(numbers.Count);

                return numbers[index];
            }

            return 0;
           
        }

      

    }
}