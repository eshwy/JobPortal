using System;
using System.Collections.Generic;

#nullable disable

namespace JopPortalMVC.Models
{
    public class PersonalDetailsTbl
    {
        public int RowId { get; set; }
        
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Experience { get; set; }
        public string Skills { get; set; }

        //public virtual LoginTbl User { get; set; }
    }
}
