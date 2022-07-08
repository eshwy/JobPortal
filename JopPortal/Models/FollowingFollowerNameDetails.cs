using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortal.Models
{
    public class FollowingFollowerNameDetails
    {
        public int RowId { get; set; }
        public int? UserId { get; set; }
        public int? FollowerId { get; set; }
        public string? Name { get; set; }
    }
}
