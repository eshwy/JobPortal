using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JopPortal.Models;
using JopPortal.Repository;
using Microsoft.AspNetCore.Authorization;

namespace JopPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginTblsController : ControllerBase
    {
        private readonly JobPortal2Context _context;
        private readonly IJWTManagerRepository _jWTManager;

        public LoginTblsController(JobPortal2Context context,IJWTManagerRepository repo)
        {
            _context = context;
            _jWTManager = repo;
        }

        // GET: api/LoginTbls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginTbl>>> GetLoginTbls()
        {
            return await _context.LoginTbls.ToListAsync();
        }

        // GET: api/LoginTbls/5
        [HttpPost]        
        [AllowAnonymous]
        public ActionResult<LoginTbl> GetLoginTbl(LoginTbl login)
        {
            var token = _jWTManager.Authenticate(login);

            if (token == null)
            {
                return Unauthorized();
            }

            var data = _context.LoginTbls.FirstOrDefault(x => x.UserName == login.UserName && x.PassWord == login.PassWord);
            var userId = data.UserId;
            var PersonalData = _context.PersonalDetailsTbl.FirstOrDefault(x => x.UserId == userId);
            if (PersonalData.Skills == null)
            {
                List<string> val = new List<string>();
                var info="PersonalDetailsNotFilled";
                val.Add(Convert.ToString(PersonalData));
                return Ok(new { token,info, PersonalData});
            };

            var WorkData = _context.UserWorkTbls.FirstOrDefault(x => x.UserId == userId);
            if (WorkData == null)
            {
                var info = "WorkDetailsNotFilled";
                return Ok(new { token,info, userId });
            };

            var EducationData = _context.UserEducationTbls.FirstOrDefault(x => x.UserId == userId);
            if(EducationData == null)
            {
                var info = "EducationDetailsNotFilled";
                return Ok(new{ token,info,userId});
            };
            
            return Ok(new { token });
        }


        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> SignUp(SignUpData signUpData)
        {
            //exec[InsertIDIntoAllTableSp]  @choice = 'Insert', @UserName = 'eshwy',PassWord = '1234',
            //@Email = 'eshwanth@gmail.com',
            //@Return = 0;
            var EmailData = _context.PersonalDetailsTbl.Where(c => c.Email == signUpData.Email);
            if(EmailData.Count() == 0)
            {
                var LoginTbl = new LoginTbl() {
                    UserName = signUpData.UserName,
                    PassWord=signUpData.PassWord
                };
                
                _context.LoginTbls.Add(LoginTbl);
                await _context.SaveChangesAsync();

                var data= CreatedAtAction("GetLoginTbl", new { id = LoginTbl.UserId }, LoginTbl);
                var d = data.Value;
                var token = _jWTManager.Authenticate((LoginTbl)d);
                var list = new List<LoginTbl>();
                
                list.Add((LoginTbl)d);
                var value = list.Select(x => x.UserId).FirstOrDefault();
                string UserId = value.ToString();

                var PersonalDetailsTbl = new PersonalDetailsTbl()
                {
                    UserId= Int32.Parse(UserId),
                    Email = signUpData.Email
                };
                _context.PersonalDetailsTbl.Add(PersonalDetailsTbl);
                await _context.SaveChangesAsync();

                var data1 = CreatedAtAction("PersonalTbl", new { id = PersonalDetailsTbl.RowId }, PersonalDetailsTbl);
                var d1 = data1.Value;
                var list1 = new List<PersonalDetailsTbl>();

                list1.Add((PersonalDetailsTbl)d1);
                var EmailRowId = list1.Select(x => x.RowId).FirstOrDefault();
                var EmailId = list1.Select(x => x.Email).FirstOrDefault();
                //string UserId1 = value1.ToString();
                //string UserId2 = value2.ToString();

                return Ok(new { token, EmailRowId, EmailId, UserId });
                //return UserId +"|" +UserId1 + "|"+ UserId2;
            }
            return BadRequest();
            
        }

        private bool LoginTblExists(int id)
        {
            return _context.LoginTbls.Any(e => e.UserId == id);
        }
    }
}
