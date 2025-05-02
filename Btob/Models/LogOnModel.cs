using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Btob.Models
{
    public partial class LogOnModel
    {
      
        [Display(Name = "Utilisateur")]
     
        [Required(ErrorMessage = "This field is required")]
        public string UserName { get; set; }

        
        
        [Display(Name = "Mdp")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}