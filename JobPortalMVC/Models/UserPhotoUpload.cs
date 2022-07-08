using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class UserPhotoUpload
    {
        
        public int? UserId { get; set; }
        
        public IFormFile photo { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
