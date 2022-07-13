using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalMVC.Models
{
    public class ArticleList
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }        
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Skills { get; set; }
        public string Email { get; set; }
    }
}
