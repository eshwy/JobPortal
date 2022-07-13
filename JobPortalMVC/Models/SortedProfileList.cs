using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalMVC.Models
{
    public class SortedProfileList
    {
        public int RowId { get; set; }
        public int SelectBy { get; set; }
        public int SortedProfile { get; set; }
        public string PhotoPath { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }        
        public int? Experience { get; set; }
        public string Skills { get; set; }
    }
}
