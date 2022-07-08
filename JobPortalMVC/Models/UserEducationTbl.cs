using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class UserEducationTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string EducationType { get; set; }
        public string GroupName { get; set; }
        public string CompletedEducationIn { get; set; }
        public int? YearOfStart { get; set; }
        public int? YearOfEnd { get; set; }
        public decimal? PercentageObtained { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
