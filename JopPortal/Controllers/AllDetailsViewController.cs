using JopPortal.Models;
using JopPortal.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllDetailsViewController : ControllerBase
    {
        private readonly JobPortal2Context _context;        
        public AllDetailsViewController(JobPortal2Context context)
        {
            _context = context;
        
        }

        [HttpGet]        
        public ActionResult DetailsView()
        {
            var data = (from t1 in _context.PersonalDetailsTbl
                        join t2 in _context.UserProfilePhotoTbls on t1.UserId equals t2.UserId
                        select new
                        {
                            UserId = t1.UserId,
                            PhotoPath = t2.PhotoPath,
                            Email = t1.Email,
                            FullName = t1.FirstName +" "+ t1.LastName,
                            PhoneNumber = t1.PhoneNumber,
                            Gender = t1.Gender,
                            DateOfBirth = t1.DateOfBirth,
                            Experience = t1.Experience,
                            Skills = t1.Skills
                        }).ToList();
            return Ok(data);
        }

    }
}
