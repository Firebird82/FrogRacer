using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrogRacer.Models;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using WorkerRole;

namespace FrogRacer.Controllers
{
    public class BettingController : Controller
    {
        string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        string qname = "frogracingqueue";
        

        public ActionResult CalculateFrogRace(int? frog1, int? frog2, int? frog3, int? frog4, int? frog5)
        {
            List<Frog> lineUpFrogs = (List<Frog>)Session["frogList"];

            Random rnd = new Random();

            var winnerFrogsNumber = rnd.Next(0,lineUpFrogs.Count);
            var winnerFrog = lineUpFrogs[winnerFrogsNumber];

            ViewBag.winnerFrog = winnerFrog;

            int newBalance = 0;

            if (frog1 != null && frog2 != null && frog3 != null && frog4 != null && frog5 != null)
            {
                newBalance = (int)Session["balance"];

                List<int?> bettingsList = new List<int?>();
                bettingsList.Add(frog1);
                bettingsList.Add(frog2);
                bettingsList.Add(frog3);
                bettingsList.Add(frog4);
                bettingsList.Add(frog5);

                bool bettedOnWinningFrog = false;
                int winningSum = 0;
                int losingSum = 0;
                ViewBag.resultUserName = (string)Session["UserName"];

                for (int i = 0; i < lineUpFrogs.Count; i++)
                {
                    if (i == winnerFrogsNumber && bettingsList[winnerFrogsNumber] != null)
                    {
                        newBalance += (int)bettingsList[winnerFrogsNumber] * 2;
                        winningSum = (int)bettingsList[winnerFrogsNumber] * 2;
                        bettedOnWinningFrog = true;
                    }

                    if (bettingsList[i] != null)
	                {
                        newBalance -= (int)bettingsList[i];
                        losingSum -= (int)bettingsList[i];
	                }
                }

                if (bettedOnWinningFrog == true)
                {
                    ViewBag.ResultMessage = "Yay, you have won " + winningSum + " and you now have " + newBalance + " left.";
                }
                else
                {
                    ViewBag.ResultMessage = "Oh no, you have lost " + losingSum + " and you now have " + newBalance + " left.";
                }
            }

            //Storage - Spara saldo börjar här

            var nm = NamespaceManager.CreateFromConnectionString(connectionString);
            QueueDescription qd = new QueueDescription(qname);         

            if (!nm.QueueExists(qname))
            {
                nm.CreateQueue(qd);
            }

            //newBalance = 100;

            //Skicka till queue med hjälp av den connectionstring vi tidigare ställt in i configen
            QueueClient qc = QueueClient.CreateFromConnectionString(connectionString, qname);

            User user = new User(Session["UserName"].ToString());
            
            //Anv. nya saldo 
            user.Balance = newBalance; // <-- Save newBalance to storage and remove this line

            //Skapa msg och skicka till QueueClient           
            var updateSaldoMsg = new BrokeredMessage();

            updateSaldoMsg.Properties["userName"] = user.UserName;
            updateSaldoMsg.Properties["balance"] = user.Balance;

            qc.Send(updateSaldoMsg);
            //Storage - Slut

            return View("Result", winnerFrog);
        }

        // GET: Betting
        public ActionResult Index()
        {
            return View();
        }

        // GET: Betting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Betting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Betting/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Betting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Betting/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Betting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Betting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
