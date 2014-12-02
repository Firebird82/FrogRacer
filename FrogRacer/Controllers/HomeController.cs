using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrogRacer.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using WorkerRole;

namespace FrogRacer.Controllers
{
    public class HomeController : Controller
    {
        //Här hämtar vi ut den connectionstring vi precis har ställt in i våra roles.
        string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        string qname = "frogracingqueue";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return View("SignUp");
            }
            else
            {
                Session["UserName"] = userName;
                //string user = (string)Session["UserName"];

                User user = new User(userName);

                //TODO Hämta saldo från Storage start

                //Om användaren inte finns i storage får den 1000
                if (Session["balance"] != null)
                {
                    user.Balance = (int)Session["balance"];
                }
                //Hämta saldo från Storage slut

                var nm = NamespaceManager.CreateFromConnectionString(connectionString);
                QueueDescription qd = new QueueDescription(qname);
                //Ställ in Max size på queue på  2GB
                qd.MaxSizeInMegabytes = 2048;
                //Max Time To Live är 5 minuter  
                qd.DefaultMessageTimeToLive = new TimeSpan(0, 5, 0);

                if (!nm.QueueExists(qname))
                {
                    nm.CreateQueue(qd);
                }

                //Skicka till queue med hjälp av den connectionstring vi tidigare ställt in i configen
                QueueClient qc = QueueClient.CreateFromConnectionString(connectionString, qname);

                //Skapa msg med email properaty och skicka till QueueClient
                var bm = new BrokeredMessage();

                Session["balance"] = user.Balance;
                bm.Properties["userName"] = user.UserName;
                bm.Properties["balance"] = user.Balance;
                qc.Send(bm);



                ViewBag.message = "Hello " + user.UserName + ".You have a balance of " + user.Balance +
                                     " USD";

                var frogData = new FrogData();
                var frogList = frogData.GetFrogList();

                Session["frogList"] = frogList;

                ViewBag.frogList = frogList;

                return View("Betting", frogList);
            }

        }


    }
}