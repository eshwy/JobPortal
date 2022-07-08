using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class UserAdressTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string CurrentDoorNumber { get; set; }
        public string CurrentStreetName { get; set; }
        public string CurrentArea { get; set; }
        public string CurrentCity { get; set; }
        public int? CurrentPinCode { get; set; }
        public string PermantentDoorNumber { get; set; }
        public string PermantentStreetName { get; set; }
        public string PermantentArea { get; set; }
        public string PermantentCity { get; set; }
        public int? PermantentPinCode { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
