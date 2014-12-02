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

            if (frog1 != null || frog2 != null || frog3 != null || frog4 != null || frog5 != null)
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

            QueueClient qc = QueueClient.CreateFromConnectionString(connectionString, qname);

            User user = new User(Session["UserName"].ToString());

            user.Balance = newBalance; 
        
            var msg = new BrokeredMessage();

            msg.Properties["userName"] = user.UserName;
            msg.Properties["balance"] = user.Balance;

            qc.Send(msg);
            //Storage - Slut

            return View("Result", winnerFrog);
        }
    }
}
