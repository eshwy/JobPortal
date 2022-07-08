using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortal.Models
{
    public class SignUpData
    {
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string PassWord { get; set; }
        [NotMapped]
        public string Email { get; set; }

    }
}
