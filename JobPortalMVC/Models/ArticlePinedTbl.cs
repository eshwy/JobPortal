using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JopPortalMVC.Models
{
    public class ArticlePinedTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public int? PinedArticleId { get; set; }
        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public string Category { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
