using System;
using System.Web.Mvc;
using System.Web.Script.Services;

namespace Btob.Controllers
{
    public class DocumentController : Controller
    {
        [HttpGet]
        public ActionResult PrintOrder(string id)
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            Sales.Service_RC service = new Sales.Service_RC();
            var orderData = service.GetSalesLines(id);
            return View("OrderPrint", orderData);
        }

        [HttpGet]
        public ActionResult PrintInvoice(string id)
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            Sales.Service_RC service = new Sales.Service_RC();
            var invoiceData = service.GetSalesLines(id);
            return View("InvoicePrint", invoiceData);
        }

        [HttpGet]
        public ActionResult PrintShipment(string id)
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            Sales.Service_RC service = new Sales.Service_RC();
            var shipmentData = service.GetSalesLines(id);
            return View("ShipmentPrint", shipmentData);
        }

        [HttpGet]
        public ActionResult PrintCreditMemo(string id)
        {
            if (Session["nom"] == null)
                return RedirectToAction("Index", "Login");

            Sales.Service_RC service = new Sales.Service_RC();
            var creditMemoData = service.GetSalesLines(id);
            return View("CreditMemoPrint", creditMemoData);
        }
    }
} 