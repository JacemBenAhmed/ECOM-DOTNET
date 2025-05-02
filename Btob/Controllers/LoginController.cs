using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.UI;
using Btob.Models;
using Newtonsoft.Json;

namespace Btob.Controllers
{
    public class LoginController : Controller
    {
        //
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult Valider()
        {

            if (Session["nom"] != null)
            {
                string re = Session["Cmd"].ToString();
                // Session["qte"] = "1";
                //  ViewBag.totalrecord = Session["qte"];
                ViewBag.nom = "";
                //   Session["nom"] = null;

                Response.Cookies["qtete"].Value = "0";// Session["qte"].ToString();
                Sales.Service_RC service = new Sales.Service_RC();
                service.UpdateSalesHeadersStatus(Session["Cmd"].ToString(), 1);
                Session["Cmd"] = null;
                /*  Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                  Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                  Response.Cache.SetNoStore();
                  Session.Abandon();

                  FormsAuthentication.SignOut();*/
                //   System.Net.AuthenticationManager.Aut();
                string result;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = JsonConvert.SerializeObject(re, Newtonsoft.Json.Formatting.Indented);
                Response.Clear();
                Response.ContentType = "application/json";

                // ViewBag.nom = login;
                Response.Write(result);
                Response.Flush();
                //  return   RedirectToAction("Accueil", "Login");

            }
            // else return RedirectToAction("Accueil", "Login");
            return new EmptyResult();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult AddToCart(string reference, string description, decimal prix, int quantite)
        {
            string re = "";
            re = "0";

            if (Session["qte"] == null)
            { Session["qte"] = ( quantite).ToString();
                Response.Cookies["qtete"].Value = Session["qte"].ToString();
            }

           else
            {
                 Session["qte"] = (int.Parse(Session["qte"].ToString())+ quantite).ToString();
                Response.Cookies["qtete"].Value = Session["qte"].ToString();

            }


            re = Session["qte"].ToString();
                string result;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            result = JsonConvert.SerializeObject(re, Newtonsoft.Json.Formatting.Indented);
            Response.Clear();
            Response.ContentType = "application/json";

            // ViewBag.nom = login;
            Response.Write(result);
            Response.Flush();

            return new EmptyResult();

            }
        public class CartItem
        {
            public string Reference { get; set; }
            public string Description { get; set; }
            public decimal Prix { get; set; }
            public int Quantite { get; set; }

            // Vous pouvez ajouter d'autres propriétés si nécessaire
            public decimal TotalPrice => Prix * Quantite;
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult DeleteItem(string NL, string Ref)
        {
            string re = "";
            re = "0";
            Sales.Service_RC service = new Sales.Service_RC();
            DataTable log = new DataTable();

            try
            {
                service.deleteItemV2(Session["Cmd"].ToString(), NL, Ref);
                re = service.GetSalesLines(Session["Cmd"].ToString()).Rows.Count.ToString();// "1";
                Response.Cookies["qtete"].Value = re;// Session["qte"].ToString();

            }
            catch { re = "0"; }
            string result;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            result = JsonConvert.SerializeObject(re, Newtonsoft.Json.Formatting.Indented);
            Response.Clear();
            Response.ContentType = "application/json";


            Response.Write(result);
            Response.Flush();

            return new EmptyResult();


        }

        // GET: /Login/
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Index()
        {
            try
            {
                if (Session["msgErreur"] != null)
                    Response.Cookies["msgErreur"].Value = Session["msgErreur"].ToString();
                else
                    Response.Cookies["msgErreur"].Value = "";
            }
            catch { }

            return View();

        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Accueil()
        {

            if (Session["nom"] != null)
            {
                // Session["qte"] = "1";
                //  ViewBag.totalrecord = Session["qte"];
                ViewBag.nom = Session["nom"].ToString();
                Response.Cookies["InfoClient"].Value = Session["nom"].ToString();
                Response.Cookies["qtete"].Value = "0";// Session["qte"].ToString();

                GetTree();
                return View();
            }
            else
            {
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Session.Abandon();

                FormsAuthentication.SignOut();
                //   System.Net.AuthenticationManager.Aut();

                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult Panier()
        {
            ViewBag.nom = Session["nom"].ToString();
            Getorder();
            return View();

        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult VerifLogin(string login, string password)
        {
            string re = "";
            re = "false";
            Sales.Service_RC service = new Sales.Service_RC();
            DataTable log = new DataTable();
            log = service.LoginBtob(login, password);
            if (log.Rows.Count > 0)
            {
                foreach (DataRow row in log.Rows)
                {
                    //  foreach (DataColumn column in dt.Columns)
                    {
                        Session["nom"] = "Bienvenue " + row[2] + " .  ";
                        re = "true";
                    }
                }
            }

            string result = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            result = JsonConvert.SerializeObject(re, Newtonsoft.Json.Formatting.Indented);
            Response.Clear();
            Response.ContentType = "application/json";

            ViewBag.nom = login;
            Response.Write(result);
            Response.Flush();

            return new EmptyResult();

        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult GetItem(string Id)
        {
            string re = "";
            re = "false";
            if (Session["nom"] == null)
            { return RedirectToAction(""); }
            else
            {
                Sales.Service_RC service = new Sales.Service_RC();
                DataTable log = new DataTable();
                log = service.Get_ItemBtob(Id, "");


                string result = "";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.Indented);
                Response.Clear();
                Response.ContentType = "application/json";

                // ViewBag.nom = login;
                Response.Write(result);
                Response.Flush();

                return new EmptyResult();
            }
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult GetItemByDesc(string ID)
        {
            string re = "";
            re = "false";
            Sales.Service_RC service = new Sales.Service_RC();
            DataTable log = new DataTable();
            log = service.GetItemByDescBTOB(ID);


            string result = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            result = JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.Indented);
            Response.Clear();
            Response.ContentType = "application/json";

            // ViewBag.nom = login;
            Response.Write(result);
            Response.Flush();

            return new EmptyResult();

        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult GetPanier()
        {
            if (Session["Cmd"] == null)
            { return RedirectToAction(""); }
            else
            {
                string re = "";
                re = "false";
                Sales.Service_RC service = new Sales.Service_RC();
                DataTable log = new DataTable();
                log = service.GetSalesLinesV2(Session["Cmd"].ToString());


                string result = "";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.Indented);
                Response.Clear();
                Response.ContentType = "application/json";

                // ViewBag.nom = login;
                Response.Write(result);
                Response.Flush();

                return new EmptyResult();
            }
        }

        void GetTree()
        {
            IList<Tree> tr = new List<Tree>();
            Sales.Service_RC service = new Sales.Service_RC();

            DataTable dt = new DataTable();
            // dt = service.GetFamille("");


            tr.Add(new Tree() { ID = 0, ID_PARENT = 1, Description = "Commandes vente" });
            tr.Add(new Tree() { ID = 0, ID_PARENT = 2, Description = " Expeditions vente" });
            tr.Add(new Tree() { ID = 0, ID_PARENT = 3, Description = "Retours vente" });
            tr.Add(new Tree() { ID = 0, ID_PARENT = 4, Description = "Factures vente " });
            tr.Add(new Tree() { ID = 0, ID_PARENT = 5, Description = "Avoir vente" });





            // foreach (DataRow row in dt.Rows)
            //  {

            //  tr.Add(new Tree() { ID = int.Parse(row[0].ToString()), ID_PARENT = int.Parse(row[5].ToString()), Description = row[2].ToString() });

            //   }

            ViewData["Menu"] = tr;

        }
        void Getorder()
        {
            IList<Order> tr = new List<Order>();
            Sales.Service_RC service = new Sales.Service_RC();

            DataTable dt = new DataTable();
            dt = service.GetSalesLinesV2(Session["Cmd"].ToString());
            /*   foreach (DataColumn col in dt.Columns)
               {
                  // ViewBag.Total = col.ColumnName;  
                   MagHeader.Add(new MagHeader() { codemgHeader = col.ColumnName });
               }

               ViewData["magHeader"] = MagHeader;*/


            foreach (DataRow row in dt.Rows)
            {

                tr.Add(new Order() { NLigne = int.Parse(row[0].ToString()), Ref = row[1].ToString(), Designation = row[2].ToString(), quantite = int.Parse(row[3].ToString()), prix = decimal.Parse(row[4].ToString()) });

            }

            ViewData["order"] = tr;

        }
        //*********************LOGOUT*****************************
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Logout()
        {
            //Session.Remove("id");

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Session.Abandon();

            FormsAuthentication.SignOut();
            //AuthenticationManager.SignOut();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Login");

            return Json(new { Url = redirectUrl }); ;
        }
        //< Business central
        [HttpPost]
        public ActionResult Logpost(LogOnModel person)
        {
            string UserName = person.UserName;
            string Password = person.Password;
            string specifier;
            CultureInfo culture;
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = "";
            specifier = "N";
            culture = CultureInfo.CreateSpecificCulture("fr-CA");
            if (p.login(UserName, Password, ref codeclient, ref desc, ref solde))
            {
                Session["nom"] = $"{desc} {solde.ToString(specifier, culture)}";
                Session["CodeClient"] = codeclient;
                return RedirectToAction("Accueil", "Login");
            }
            else
            {

                // Session["msgErreur"] = desc;
                //Response.Cookies["msgErreur"].Value = desc;
                return RedirectToAction("Index", "Login");


                /* Sales.Service_RC service = new Sales.Service_RC();
                 DataTable log = new DataTable();
                 log = service.LoginBtob(UserName, Password);
                 if (log.Rows.Count > 0)
                 {
                     foreach (DataRow row in log.Rows)
                     {
                         //  foreach (DataColumn column in dt.Columns)
                         {
                             Session["nom"] = "Bienvenue " + row[2] + " .  ";
                             return RedirectToAction("Accueil", "Login");
                         }
                     }
                 }*/
                return View();
            }
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult affichercommande()
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesHeaders SalesHeader = new PortailWeb.SalesHeaders();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportOrderHeader(ref SalesHeader, codeclient, "");
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Numéro");
            dt.Columns.Add("Nom");
            dt.Columns.Add("Numéro du client");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TTC");
            dt.Columns.Add("Date de comptabilisation");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < SalesHeader.SalesHeader.Length; i++)
            {
                dt.Rows.Add(
                     SalesHeader.SalesHeader[i].No.ToString(),
                     SalesHeader.SalesHeader[i].SelltoCustomerName.ToString(),
                     SalesHeader.SalesHeader[i].SelltoCustomerNo.ToString(),
                     SalesHeader.SalesHeader[i].Amount.ToString(),
                     SalesHeader.SalesHeader[i].AmountIncludingVAT.ToString(),
                     SalesHeader.SalesHeader[i].PostingDate.ToString());

            }
            //remplir datatable -> js
            var salesData = SalesHeader.SalesHeader.Select(header => new
            {
                Numero = header.No,
                Nom = header.SelltoCustomerName,
                NumeroClient = header.SelltoCustomerNo,
                Montant = header.Amount,
                MontantTTC = header.AmountIncludingVAT,
                Date = header.PostingDate.ToString("yyyy-MM-dd"),
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult ExpeditionVente()
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            PortailWeb.SalesShipmentHeaders ShipmentList = new PortailWeb.SalesShipmentHeaders();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportShipmentHeader(ref ShipmentList, codeclient, "");

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("N° Client");
            dt.Columns.Add("Nom client");
            dt.Columns.Add("date");

            DataRow row = dt.NewRow();
            dt.Rows.Add(row);

            for (int i = 0; i < ShipmentList.SalesShipmentHeader.Length; i++)
            {
                dt.Rows.Add(
                     ShipmentList.SalesShipmentHeader[i].No.ToString(),
                     ShipmentList.SalesShipmentHeader[i].SelltoCustomerNo.ToString(),
                     ShipmentList.SalesShipmentHeader[i].SelltoCustomerName.ToString(),
                     ShipmentList.SalesShipmentHeader[i].PostingDate.ToString());
            }

            var salesData = ShipmentList.SalesShipmentHeader.Select(header => new
            {
                Numero = header.No,
                Nom = header.SelltoCustomerNo,
                NumeroClient = header.SelltoCustomerName,
                Date = header.PostingDate.ToString("yyyy-MM-dd"),
            }).ToList();

            return Json(salesData, JsonRequestBehavior.AllowGet);
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult facturevente()
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesInvoiceHeaders InvoiceHeader = new PortailWeb.SalesInvoiceHeaders();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportInvoice(ref InvoiceHeader, codeclient, "");
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("N° Client");
            dt.Columns.Add("Nom client");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TVA");
            dt.Columns.Add("Date");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < InvoiceHeader.SalesInvoiceHeader.Length; i++)
            {
                dt.Rows.Add(
                     InvoiceHeader.SalesInvoiceHeader[i].No.ToString(),
                     InvoiceHeader.SalesInvoiceHeader[i].SelltoCustomerNo.ToString(),
                     InvoiceHeader.SalesInvoiceHeader[i].SelltoCustomerName.ToString(),
                     InvoiceHeader.SalesInvoiceHeader[i].Amount.ToString(),
                     InvoiceHeader.SalesInvoiceHeader[i].AmountIncludingVAT.ToString(),
                     InvoiceHeader.SalesInvoiceHeader[i].PostingDate.ToString());
            }
            //remplir datatable -> js
            var salesData = InvoiceHeader.SalesInvoiceHeader.Select(header => new
            {
                Numero = header.No,
                NumeroClient = header.SelltoCustomerNo,
                Nom = header.SelltoCustomerName,
                Montant = header.Amount,
                MontantTVA = header.AmountIncludingVAT,
                Date = header.PostingDate.ToString("yyyy-MM-dd"),
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
    
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult avoirvente()
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesCrMemoHeaders AvoirHeader = new PortailWeb.SalesCrMemoHeaders();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportAvoir(ref AvoirHeader, codeclient, "");
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("N° Client");
            dt.Columns.Add("Nom client");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TVA");
            dt.Columns.Add("Date");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < AvoirHeader.SalesCrMemoHeader.Length; i++)
            {
                dt.Rows.Add(
                     AvoirHeader.SalesCrMemoHeader[i].No.ToString(),
                     AvoirHeader.SalesCrMemoHeader[i].SelltoCustomerNo.ToString(),
                     AvoirHeader.SalesCrMemoHeader[i].SelltoCustomerName.ToString(),
                     AvoirHeader.SalesCrMemoHeader[i].Amount.ToString(),
                     AvoirHeader.SalesCrMemoHeader[i].AmountIncludingVAT.ToString(),
                     AvoirHeader.SalesCrMemoHeader[i].PostingDate.ToString());



            }
            //remplir datatable -> js
            var salesData = AvoirHeader.SalesCrMemoHeader.Select(header => new
            {
                Numero = header.No,
                NumeroClient = header.SelltoCustomerNo,
                Nom = header.SelltoCustomerName,
                Montant = header.Amount,
                MontantTVA = header.AmountIncludingVAT,
                Date = header.PostingDate.ToString("yyyy-MM-dd"),
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult DetailCdeVente(String ID) // no commande en parametre
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesLines salesline = new PortailWeb.SalesLines();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportOrderLine(ref salesline, ID);
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantité");
            dt.Columns.Add("Prix Unitaire");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TVA");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < salesline.SalesLine.Length; i++)
            {
                dt.Rows.Add(
                     salesline.SalesLine[i].No.ToString(),
                     salesline.SalesLine[i].Description.ToString(),
                     salesline.SalesLine[i].Quantity.ToString(),
                     salesline.SalesLine[i].UnitPrice.ToString(),
                     salesline.SalesLine[i].Amount.ToString(),
                     salesline.SalesLine[i].AmountIncludingVAT.ToString());
            }
            //remplir datatable -> js
            var salesData = salesline.SalesLine.Select(header => new
            {
                Numero = header.No,
                Description = header.Description,
                Quantite = header.Quantity,
                Prix = header.UnitPrice,
                Montant = header.Amount,
                MontantTVA = header.AmountIncludingVAT,
            }) .ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult DetailExpVente(String ID) // no commande en parametre
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesShipementLines SalesShipmentLine = new PortailWeb.SalesShipementLines();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportShipementLine(ref SalesShipmentLine, ID);
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantité");
            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < SalesShipmentLine.SalesShipmentLine.Length; i++)
            {
                dt.Rows.Add(
                     SalesShipmentLine.SalesShipmentLine[i].No.ToString(),
                     SalesShipmentLine.SalesShipmentLine[i].Description.ToString(),
                     SalesShipmentLine.SalesShipmentLine[i].Quantity.ToString());
                  
            }
            //remplir datatable -> js
            var salesData = SalesShipmentLine.SalesShipmentLine.Select(header => new
            {
                Numero = header.No,
                Description = header.Description,
                Quantite = header.Quantity,
            
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult DetailFactureVente(String ID) // no commande en parametre
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesInvoiceLines SalesInvoiceLine = new PortailWeb.SalesInvoiceLines();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportInvoiceLine(ref SalesInvoiceLine, ID);
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantité");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TVA");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < SalesInvoiceLine.SalesInvoiceLine.Length; i++)
            {
                dt.Rows.Add(
                     SalesInvoiceLine.SalesInvoiceLine[i].No.ToString(),
                     SalesInvoiceLine.SalesInvoiceLine[i].Description.ToString(),
                     SalesInvoiceLine.SalesInvoiceLine[i].Quantity.ToString(),
                     SalesInvoiceLine.SalesInvoiceLine[i].Amount.ToString(),
                     SalesInvoiceLine.SalesInvoiceLine[i].AmountIncludingVAT.ToString());
            }
            //remplir datatable -> js
            var salesData = SalesInvoiceLine.SalesInvoiceLine.Select(header => new
            {
                Numero = header.No,
                Description = header.Description,
                Quantite = header.Quantity,
                Montant = header.Amount,
                MontantTVA = header.AmountIncludingVAT,
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        public ActionResult DetailAvoirVente(String ID) // no commande en parametre
        {
            PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
            p.UseDefaultCredentials = true;
            p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");//app.config
            string desc = "";
            decimal solde = 0;
            string codeclient = Session["CodeClient"].ToString();

            //   p.login("user", "1234", ref codeclient, ref desc, ref solde);
            PortailWeb.SalesCrMemoLines SalesCrMemoLine = new PortailWeb.SalesCrMemoLines();
            DateTime debut = new DateTime(2020, 1, 1);
            DateTime fin = new DateTime(2026, 1, 1);
            p.ExportAvoirLine(ref SalesCrMemoLine, ID);
            // dt = SalesHeader;
            //remplir datatable xmlport-> datatable
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("N°");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantité");
            dt.Columns.Add("Montant");
            dt.Columns.Add("Montant TVA");

            DataRow row = dt.NewRow();

            dt.Rows.Add(row);
            for (int i = 0; i < SalesCrMemoLine.SalesCrMemoLine.Length; i++)
            {
                dt.Rows.Add(
                     SalesCrMemoLine.SalesCrMemoLine[i].No.ToString(),
                     SalesCrMemoLine.SalesCrMemoLine[i].Description.ToString(),
                     SalesCrMemoLine.SalesCrMemoLine[i].Quantity.ToString(),
                     SalesCrMemoLine.SalesCrMemoLine[i].Amount.ToString(),
                     SalesCrMemoLine.SalesCrMemoLine[i].AmountIncludingVAT.ToString());
            }
            //remplir datatable -> js
            var salesData = SalesCrMemoLine.SalesCrMemoLine.Select(header => new
            {
                Numero = header.No,
                Description = header.Description,
                Quantite = header.Quantity,
                Montant = header.Amount,
                MontantTVA = header.AmountIncludingVAT,
            }).ToList();
            return Json(salesData, JsonRequestBehavior.AllowGet);
            //  return View();
        }
        public ActionResult ImprimerCdeVente(string ID)
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return new HttpStatusCodeResult(400, "ID de commande manquant");
                }

                PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
                p.UseDefaultCredentials = true;
                p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");

                string codeclient = Session["CodeClient"]?.ToString();
                if (string.IsNullOrEmpty(codeclient))
                {
                    return new HttpStatusCodeResult(401, "Utilisateur non authentifié");
                }

                string pdf = "";
                bool execute = false;
                p.ReportToPDF(ref pdf, ref execute, 36, ID);
             //   p.ExportPictureToBase64(ref pdf, "1000");
             ///   p.ExportItems("Bic",ref pdf);
                if (!execute || string.IsNullOrEmpty(pdf))
                {
                    return new HttpStatusCodeResult(500, "Erreur lors de la génération du PDF");
                }

                byte[] pdfBytes = Convert.FromBase64String(pdf);

               return File(pdfBytes, "application/pdf", "commande_" + ID + ".pdf");
               // return File(pdfBytes, "application/png", "1000.png");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Erreur interne : " + ex.Message);
            }
        }

        public ActionResult ImprimerExpedition(string ID)
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return new HttpStatusCodeResult(400, "ID de livraison manquant");
                }

                PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
                p.UseDefaultCredentials = true;
                p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");

                string codeclient = Session["CodeClient"].ToString();
                if (string.IsNullOrEmpty(codeclient))
                {
                    return new HttpStatusCodeResult(401, "Utilisateur non authentifié");
                }

                string pdf = "";
                bool execute = false;
                p.ReportToPDF(ref pdf, ref execute, 110, ID);

                if (!execute || string.IsNullOrEmpty(pdf))
                {
                    return new HttpStatusCodeResult(500, "Erreur lors de la génération du PDF");
                }

                byte[] pdfBytes = Convert.FromBase64String(pdf);

                return File(pdfBytes, "application/pdf", "Expedition_"+ ID +".pdf");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Erreur interne : " + ex.Message);
            }
        }
        public ActionResult ImprimerFacture(string ID)
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return new HttpStatusCodeResult(400, "ID de facture manquant");
                }

                PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
                p.UseDefaultCredentials = true;
                p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");

                string codeclient = Session["CodeClient"].ToString();
                if (string.IsNullOrEmpty(codeclient))
                {
                    return new HttpStatusCodeResult(401, "Utilisateur non authentifié");
                }

                string pdf = "";
                bool execute = false;
                p.ReportToPDF(ref pdf, ref execute, 112, ID);

                if (!execute || string.IsNullOrEmpty(pdf))
                {
                    return new HttpStatusCodeResult(500, "Erreur lors de la génération du PDF");
                }

                byte[] pdfBytes = Convert.FromBase64String(pdf);

                return File(pdfBytes, "application/pdf", "facture_" + ID + ".pdf");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Erreur interne : " + ex.Message);
            }
        }

        public ActionResult ImprimerAvoir(string ID)
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    return new HttpStatusCodeResult(400, "ID d'avoir manquant");
                }

                PortailWeb.PortailWeb p = new PortailWeb.PortailWeb();
                p.UseDefaultCredentials = true;
                p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");

                string codeclient = Session["CodeClient"].ToString();
                if (string.IsNullOrEmpty(codeclient))
                {
                    return new HttpStatusCodeResult(401, "Utilisateur non authentifié");
                }

                string pdf = "";
                bool execute = false;
                p.ReportToPDF(ref pdf, ref execute, 114, ID);

                if (!execute || string.IsNullOrEmpty(pdf))
                {
                    return new HttpStatusCodeResult(500, "Erreur lors de la génération du PDF");
                }

                byte[] pdfBytes = Convert.FromBase64String(pdf);

                return File(pdfBytes, "application/pdf", "avoir_" + ID + ".pdf");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Erreur interne : " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult DevisCommande(int devis, int commande)
        {
            Session["d"] = devis;
            Session["c"] = commande;

            return Json(new { success = true, message = "Valeurs enregistrées avec succès !" });
        }
        [HttpGet]

        public ActionResult Recherche(string id)
        {
            if (Session["nom"] == null)
            {
                return Json(new { error = "Session expirée." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var p = new PortailWeb.PortailWeb();
                p.UseDefaultCredentials = true;
                p.Credentials = new System.Net.NetworkCredential("Yahya", "147096", "desktop-47nbubl");

                string desc = id;
                string varJSON = "";
                p.ExportItems(id, ref varJSON);

                if (string.IsNullOrWhiteSpace(varJSON))
                {
                    return Json(new { error = "Aucun article trouvé." }, JsonRequestBehavior.AllowGet);
                }

                return Content(varJSON, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { error = "Erreur : " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //>Business central

    }
}
