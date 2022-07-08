using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JopPortalMVC.Models
{
    public class UserProfilePhotoTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string PhotoPath { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
