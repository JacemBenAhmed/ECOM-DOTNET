using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Btob.Models
{
    public class Order
    {
        public int NLigne { get; set; }
        public string Ref { get; set; }
        public string Designation { get; set; }
        public int quantite { get; set; }
        public decimal prix { get; set; }
    }
}