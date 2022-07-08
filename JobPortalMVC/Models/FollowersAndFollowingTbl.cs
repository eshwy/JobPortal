using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class FollowersAndFollowingTbl
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public int? FollowerId { get; set; }
    }
}
