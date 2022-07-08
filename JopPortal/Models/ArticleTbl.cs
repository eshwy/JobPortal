using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortal.Models
{
    public class ArticleTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
