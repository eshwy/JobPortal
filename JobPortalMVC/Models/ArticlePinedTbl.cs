using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class ArticlePinedTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public int? PinedArticleId { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
