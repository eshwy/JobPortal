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
            //Article Pinned Data
            var articlePinnedDetails = await _context.ArticlePinedTbls.Where(x => x.UserId == userId).ToListAsync();

            //Article List for getting other data
            List<int> ArticleRowId = new List<int>();
            foreach (var Artdetails in articlePinnedDetails)
            {
                ArticleRowId.Add((int)Artdetails.PinedArticleId);
            }
            
            //Getting Article Table  
            List<ArticleTbl> data = new List<ArticleTbl>();
            foreach (var Idvalue in ArticleRowId)
            {
                var ArticleTitleData = await _context.ArticleTbls.Where(x => x.RowId == Idvalue).ToListAsync();
                foreach (var item1 in ArticleTitleData)
                {
                    data.Add(item1);
                }

            }
            var AdticleTableDetails = from t1 in articlePinnedDetails
                                      join t2 in data on
                                      t1.PinedArticleId equals t2.RowId
                                      select new { 
                                      t1.RowId,
                                      t1.UserId,
                                      t1.PinedArticleId,
                                      t2.Category,
                                      ArticleRowId=t2.RowId                                      
                                      };

            //Article Title Details.
            List < ArticleTitleTbl > data1 = new List<ArticleTitleTbl>();
            foreach (var Idvalue in ArticleRowId)
            {
                var ArticleTitleData = await _context.ArticleTitleTbl.Where(x => x.ArticleId == Idvalue).ToListAsync();
                foreach (var item1 in ArticleTitleData)
                {
                    data1.Add(item1);
                }

            }
            var FinalTitleData = from t1 in AdticleTableDetails
                                 join t2 in data1
                                 on t1.ArticleRowId equals t2.ArticleId
                                 group t2.Title by t1.ArticleRowId into g
                                 select new
                                 {
                                     id = g.Key,
                                     Title = string.Join(",", g.ToArray())
                                 };

            var FinalArticlePinnedData = (from t1 in AdticleTableDetails
                                    join t2 in FinalTitleData
                                    on t1.ArticleRowId equals t2.id
                                    select new
                                    {
                                        t1.PinedArticleId,
                                        t1.RowId,
                                        t1.UserId,
                                        t2.Title,                                        
                                        t1.Category
                                    }).ToList();


            return Ok(FinalArticlePinnedData);
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
            List<int> SortedSelectId = new List<int>();
            foreach (var selectId in details)
            {
                SortedSelectId.Add(selectId.SortedProfile);
            }

            List<PersonalDetailsTbl> data = new List<PersonalDetailsTbl>();
            List<UserProfilePhotoTbl> Photodata = new List<UserProfilePhotoTbl>();
            foreach (var Idvalue in SortedSelectId)
            {
                var ArticleTitleData = await _context.PersonalDetailsTbl.Where(x => x.UserId == Idvalue).ToListAsync();
                foreach (var item1 in ArticleTitleData)
                {
                    data.Add(item1);
                }
                var PhotoData = await _context.UserProfilePhotoTbls.Where(x => x.UserId == Idvalue).ToListAsync();
                foreach (var item1 in PhotoData)
                {
                    Photodata.Add(item1);
                }
            }

            var photoDetails = (from t1 in details
                                join t2 in Photodata
                                on t1.SortedProfile equals t2.UserId
                                select new
                                {
                                    t1.RowId,
                                    t1.SelectBy,
                                    t1.SortedProfile,
                                    t2.PhotoPath
                                }).ToList();

            var finalData = (from t1 in photoDetails
                             join t2 in data
                             on t1.SortedProfile equals t2.UserId
                             select new
                             {
                                 t1.RowId,
                                 t1.SelectBy,
                                 t1.SortedProfile,
                                 t1.PhotoPath,
                                 Name = t2.FirstName + " "+t2.LastName,
                                 t2.Email,
                                 t2.Skills,
                                 t2.Experience
                             }).ToList();
            
            return Ok(finalData);
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
