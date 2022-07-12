using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JopPortal.Models;
using Microsoft.AspNetCore.Authorization;

namespace JopPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //public class ArticleController : GenericCollectionController<ArticleTbl>
    //{
    //    private readonly JobPortal2Context _context;
    //    public ArticleController(JobPortal2Context context) : base(context)
    //    {
    //        _context = context;
    //    }
    //    [HttpGet("{userId}")]
    //    public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
    //    {
    //        var details = await _context.ArticleTbls.Where(x => x.UserId == userId || x.RowId==userId).ToListAsync();
    //        return Ok(details);
    //    }
        
    //}

    public class ArticlePinedController : GenericCollectionController<ArticlePinedTbl>
    {
        private readonly JobPortal2Context _context;
        public ArticlePinedController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.ArticlePinedTbls.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }
    }

    public class FollowersAndFollowingController : GenericCollectionController<FollowersAndFollowingTbl>
    {
        private readonly JobPortal2Context _context;
        public FollowersAndFollowingController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var Followers = await _context.FollowersAndFollowingTbls.Where(x => x.UserId == userId).ToListAsync();
            var Following = await _context.FollowersAndFollowingTbls.Where(x => x.FollowerId == userId).ToListAsync();

            var UserDetails = await _context.PersonalDetailsTbl.ToListAsync();           

            var details = from t1 in Followers
                          from t2 in UserDetails
                    where (t1.FollowerId == t2.UserId)
                    select new
                    {
                        RowId = t1.RowId,
                        UserId=t1.UserId,
                        FollowerId=t1.FollowerId,
                        Name =t2.FirstName+" "+t2.LastName
                    };
            var details1 = from t1 in Following
                           from t2 in UserDetails
                          where (t1.UserId == t2.UserId)
                          select new
                          {
                              RowId = t1.RowId,
                              UserId = t1.UserId,
                              FollowerId = t1.FollowerId,
                              Name = t2.FirstName + " " + t2.LastName
                          };
            List<FollowingFollowerNameDetails> followAndFollowingDetails = new List<FollowingFollowerNameDetails>();
            foreach (var item in details)
            {
                followAndFollowingDetails.Add(new FollowingFollowerNameDetails
                {
                    RowId = item.RowId,
                    UserId = item.UserId,
                    FollowerId = item.FollowerId,
                    Name = item.Name
                });
            }
            foreach (var item1 in details1)
            {
                followAndFollowingDetails.Add(new FollowingFollowerNameDetails
                {
                    RowId = item1.RowId,
                    UserId = item1.UserId,
                    FollowerId = item1.FollowerId,
                    Name = item1.Name
                });
            }
            return Ok(followAndFollowingDetails);
        }
    }

    public class PersonalDetailsController : GenericCollectionController<PersonalDetailsTbl>
    {
        private readonly JobPortal2Context _context;
        public PersonalDetailsController(JobPortal2Context context) : base(context)
        {
            _context = context;        
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.PersonalDetailsTbl.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }        
    }

    public class SortedProfilesController : GenericCollectionController<SortedProfilesTbl>
    {
        private readonly JobPortal2Context _context;
        public SortedProfilesController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.SortedProfilesTbls.Where(x => x.SelectBy == userId).ToListAsync();
            return Ok(details);
        }
    }

    public class UserAdressController : GenericCollectionController<UserAdressTbl>
    {
        private readonly JobPortal2Context _context;
        public UserAdressController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.UserAdressTbls.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }
    }

    public class UserEducationController : GenericCollectionController<UserEducationTbl>
    {
        private readonly JobPortal2Context _context;
        public UserEducationController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.UserEducationTbls.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }
    }

    public class UserProfilePhotoController : GenericCollectionController<UserProfilePhotoTbl>
    {
        private readonly JobPortal2Context _context;
        public UserProfilePhotoController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.UserProfilePhotoTbls.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }
    }

    public class UserWorkController : GenericCollectionController<UserWorkTbl>
    {
        private readonly JobPortal2Context _context;
        public UserWorkController(JobPortal2Context context) : base(context) 
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> DetailsOfParticularUsingUserId(int userId)
        {
            var details = await _context.UserWorkTbls.Where(x => x.UserId == userId).ToListAsync();
            return Ok(details);
        }
    }
}
