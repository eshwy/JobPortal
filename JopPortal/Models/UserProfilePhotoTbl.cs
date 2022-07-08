using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortal.Models
{
    public class UserProfilePhotoTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public string PhotoPath { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
