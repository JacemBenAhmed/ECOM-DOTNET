using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Btob.Models;

namespace Btob.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
       /* public ActionResult Contact(string name = "h", int numTimes = 1)
        {
            ViewBag.Message = "Hello bizite ";// +name;
          //  ViewBag.NumTimes = "brownise";

            return View("Contact?name=" + name + "&numtimes="+numTimes);
        }*/
        DataTable tree(string espace)
        {
            /*DataTable dt = new DataTable();
            // d’abord définir les colonnes de ce DataTable
            // première colonne de données : NOM, chaîne de caractères
            DataColumn dc = new DataColumn("Code", typeof(string)); dt.Columns.Add(dc);

            dc = new DataColumn("Nom", typeof(string)); dt.Columns.Add(dc);
            // dc = new DataColumn("Comptage", typeof(string)); dt.Columns.Add(dc);

            */
            
         
         /*   int totalRec = 0;
            string JsonFile = "";

         
            if (JsonFile.Length > 2)
            {
                int j = 0;
                int i = 0;
                string c3 = "";
                char delimiter = ',';
                String[] substrings = JsonFile.Split(delimiter);
                int longueur = substrings.Length;
                DataRow row;
                while (i < (longueur))
                {
                    row = dt.NewRow();
                    //ROW 0
                    string s1 = substrings[i].ToString();
                    char delimiter2 = ':';
                    String[] substrings2 = s1.Split(delimiter2);
                    string CodInventaire = substrings2[1].ToString();

                    char delimiterInventaire = '"';
                    String[] substringsInvent = CodInventaire.Split(delimiterInventaire);
                    row[0] = substringsInvent[1].ToString();


                    i = i + 1;

                    //row[1]
                    string s2 = substrings[i].ToString();
                    char delimiter3 = ':';
                    String[] substrings3 = s2.Split(delimiter3);
                    string CodMagasin = substrings3[1].ToString();

                    char delimiterMagasin = '"';
                    String[] substringsMag = CodMagasin.Split(delimiterMagasin);
                    row[1] = substringsMag[1].ToString();

                    i = i + 1;


                    dt.Rows.Add(row);


                }
            }*/
            Sales.Service_RC service = new Sales.Service_RC();
        
            return service.GetFamille_RestaurentCaf(espace);
        }
        public ActionResult Index( )
        {
            IList<Tree> tr = new List<Tree>();
           // IList<MagHeader> MagHeader = new List<MagHeader>(); 
            DataTable dt = new DataTable();
            dt = tree("100000");
         /*   foreach (DataColumn col in dt.Columns)
            {
               // ViewBag.Total = col.ColumnName;  
                MagHeader.Add(new MagHeader() { codemgHeader = col.ColumnName });
            }

            ViewData["magHeader"] = MagHeader;*/


           foreach (DataRow row in dt.Rows)
           {

               tr.Add(new Tree() { ID = int.Parse(row[0].ToString()), ID_PARENT = int.Parse(row[1].ToString()), Description = row[2].ToString() });

            }

           ViewData["cafe"] = tr;
         //  ViewBag.Total = dt.Rows.Count.ToString();
          /* List<string> dept = new List<string>() { "HR", "Sales", "Developer" };

           ViewBag.dept_name = dept;*/
         //  ViewBag.Message = ch;
           // ViewBag.numTimes = dt.Rows.Count;// numTimes;
            return View("Contact");
        }

    }
}
