using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortal.Models
{
    public partial class UserWorkTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string CompanyName { get; set; }
        public int? WorkedFrom { get; set; }
        public int? WorkedTill { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
