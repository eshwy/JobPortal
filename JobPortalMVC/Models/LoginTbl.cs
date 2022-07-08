using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JopPortalMVC.Models
{
    public class LoginTbl
    {      
        [Key]
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

   
    }
}
