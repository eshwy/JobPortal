using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JopPortalMVC.Models
{
    public class ArticleTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        [NotMapped]
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
