using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class SignUpData
    {        
        public string UserName { get; set; }        
        public string PassWord { get; set; }

        [NotMapped] 
        [Compare("PassWord")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
