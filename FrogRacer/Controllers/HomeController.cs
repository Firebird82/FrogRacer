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
                ViewBag.ErrorMessage = "You have to enter a name in the sign up box, stupid.";
                return View("Index");
            }
            else
            {
                User user = new User(userName);

                if (Session["balance"] != null && userName == (string)Session["UserName"])
                {
                    user.Balance = (int)Session["balance"];
                }

                Session["UserName"] = userName;

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
                bm.Properties["action"] = "create";
                qc.Send(bm);

                ViewBag.message = "Hello " + user.UserName + ".You have a balance of " + user.Balance + " USD";

                var frogData = new FrogData();
                var frogList = frogData.GetFrogList();

                Session["frogList"] = frogList;

                ViewBag.frogList = frogList;

                return View("Betting", frogList);
            }
        }

        public ActionResult RemoveUser()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            string qname = "frogracingqueue";

            var nm = NamespaceManager.CreateFromConnectionString(connectionString);
            QueueDescription qd = new QueueDescription(qname);

            if (!nm.QueueExists(qname))
            {
                nm.CreateQueue(qd);
            }

            QueueClient qc = QueueClient.CreateFromConnectionString(connectionString, qname);

            User user = new User(Session["UserName"].ToString());

            var msg = new BrokeredMessage();

            msg.Properties["userName"] = user.UserName;
            msg.Properties["balance"] = Session["balance"].ToString();
            msg.Properties["action"] = "delete";
            qc.Send(msg);

            return View("RemoveUser");
        }

        public ActionResult UserLeftTxtBoxesEmpty()
        {
            List<Frog> lineUpFrogs = (List<Frog>)Session["frogList"];
            string user = (string)Session["UserName"];
            int currentBalance = (int)Session["balance"];

            ViewBag.frogList = lineUpFrogs;
            ViewBag.ErrorMessage = "You forgot to place a bet, try again " + user + ". Your saldo is " + currentBalance;

            return View("Betting", lineUpFrogs);
        }

    }
}