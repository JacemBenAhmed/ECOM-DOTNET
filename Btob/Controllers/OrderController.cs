using System;
using System.Data;
using System.Web.Mvc;
using System.Web.Script.Services;
using Btob.Models;
using Newtonsoft.Json;

namespace Btob.Controllers
{
    public class OrderController : Controller
    {
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult GetOrder()
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            Sales.Service_RC service = new Sales.Service_RC();
            var orders = service.GetSalesLines(Session["Cmd"]?.ToString() ?? string.Empty);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCart()
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.nom = Session["nom"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(string reference, string description, decimal prix, int quantite)
        {
            if (Session["nom"] == null)
                return Json(new { success = false, message = "Not logged in" });

            if (Session["qte"] == null)
            {
                Session["qte"] = quantite.ToString();
            }
            else
            {
                Session["qte"] = (int.Parse(Session["qte"].ToString()) + quantite).ToString();
            }
            
            Response.Cookies["qtete"].Value = Session["qte"].ToString();
            return Json(new { success = true, quantity = Session["qte"] });
        }

        [HttpPost]
        public ActionResult DeleteItem(string orderLineNo, string reference)
        {
            if (Session["nom"] == null)
                return Json(new { success = false, message = "Not logged in" });

            try
            {
                Sales.Service_RC service = new Sales.Service_RC();
                service.deleteItemV2(Session["Cmd"].ToString(), orderLineNo, reference);
                var remainingItems = service.GetSalesLines(Session["Cmd"].ToString()).Rows.Count;
                Response.Cookies["qtete"].Value = remainingItems.ToString();
                return Json(new { success = true, itemCount = remainingItems });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 