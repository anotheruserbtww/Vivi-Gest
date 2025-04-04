using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViviGest.Utilities;

namespace ViviGest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult carlos()
        {
            return View();
        }
        public ActionResult About()
        {
            DBContextUtility dbUtility = new DBContextUtility();
            bool isConnected = dbUtility.Connect();  // Intenta conectar a la base de datos

            ViewBag.ConnectionStatus = isConnected ? "Todo Melo" : "No hay conexión";  // Envía el estado de la conexión a la vista
            dbUtility.Disconnect();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}