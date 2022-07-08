using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JopPortalMVC.Models;

namespace JobPortalMVC.Models
{
    public class ViewModelData
    {
        public IEnumerable<UserProfilePhotoTbl> UserProfilePhotoTbl { get; set; }
        public IEnumerable<PersonalDetailsTbl> PersonalDetailsTbl { get; set; }
        public IEnumerable<UserAdressTbl> AddressDetailsTbl { get; set; }
        public IEnumerable<UserEducationTbl> UserEducationTbl { get; set; }
        public IEnumerable<UserWorkTbl> UserWorkTbl { get; set; }     
        public IEnumerable<FollowingFollowerNameDetails> UserFollowingDetails { get; set; }
        public IEnumerable<FollowingFollowerNameDetails> UserFollowersDetails { get; set; }
        public IEnumerable<ArticleTbl> ArticleTbl { get; set; }
        public IEnumerable<ArticlePinedTbl> ArticlePinedTbl { get; set; }
    }
}
