using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["CurrentPage"] = 1;

            return View();
        }

        public ActionResult Products(int page, int cat)
        {
            TiendaDBHandle tienda = new TiendaDBHandle();
            var data = tienda.GetProducts(page, cat);
            Session["CurrentPage"] = page;

            ViewBag.Products = data;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection form)
        {
            Feedback feedback = new Feedback();
            feedback.Nombre = form["Nombre"];
            feedback.Email = form["Email"];
            feedback.FeedbackTypeId = Convert.ToInt32(form["FeedbackTypeId"]);
            feedback.Nota = form["Nota"];


            Models.TiendaDBHandle tiendaDB = new TiendaDBHandle();
            tiendaDB.NewFeedback(feedback);

            return View();
        }
    }
}