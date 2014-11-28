using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            User _user = new User(userName);

            if (string.IsNullOrEmpty(userName))
            {
                return View("SignUp");
            }
            else
            {
                Session["UserName"] = userName;

                string user = (string)Session["UserName"];

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
                bm.Properties["userName"] = _user.UserName;
                bm.Properties["balance"] = _user.Balance;
                qc.Send(bm);

                ViewBag.message = "Hej " + _user.UserName + ".Du har ett välkomstslado på " + _user.Balance;

                return View("Betting");
            }

        }
    }
}