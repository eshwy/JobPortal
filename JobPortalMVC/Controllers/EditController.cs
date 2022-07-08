using JopPortalMVC.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JopPortalMVC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Net.Http.Headers;
using System.IO;

namespace JobPortalMVC.Controllers
{
    public class EditController : Controller
    {
        private readonly ILogger<EditController> _logger;
        private IJobPortalUrl _jobPortalUrl;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EditController(ILogger<EditController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _jobPortalUrl = new JobPortalUrl();
            _hostEnvironment = hostEnvironment;
        }
        private StringContent Serialization(object data)
        {
            string UserLoginDetails = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(UserLoginDetails, Encoding.UTF8, "application/json");
            return content;
        }

        private string UserClaim()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var data = decodedValue.Claims.First(c => c.Type == "nameid").Value;
            return data;
        }

        private HttpClient TokenValue()
        {
            var token = HttpContext.Session.GetString("JWToken");
            HttpClient cli = _jobPortalUrl.initial();
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return cli;
        }

        [HttpPost]
        public async Task<IActionResult> EditPhoto(UserProfilePhotoTbl photoDetails)
        {
            HttpClient cli = _jobPortalUrl.initial();                        

            ViewBag.oldPath = photoDetails.PhotoPath;
            if (photoDetails.Photo != null)
            {
                string extension = Path.GetExtension(photoDetails.Photo.FileName);
                string CurrentDateTime = DateTime.Now.ToString();
                CurrentDateTime = CurrentDateTime.Replace(" ", "_").Replace("/", "_").Replace(":", "_");
                photoDetails.PhotoPath = "/Image/" + photoDetails.UserId + "_" + CurrentDateTime + extension;

                
                StringContent content = Serialization(photoDetails);
                var response = await cli.PostAsync(cli.BaseAddress + "api/UserProfilePhoto/Put", content);

                if (response.IsSuccessStatusCode)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string oldPath = Path.Combine(wwwRootPath + ViewBag.oldPath);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    string path = Path.Combine(wwwRootPath + photoDetails.PhotoPath);

                    await photoDetails.Photo.CopyToAsync(new FileStream(path, FileMode.Create));
                    return RedirectToAction("ParticularView", "Home", new { UserId = photoDetails.UserId });
                }
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = photoDetails.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> EditPersonal(PersonalDetailsTbl personalDetails)
        {
            HttpClient cli = _jobPortalUrl.initial();            
            StringContent content = Serialization(personalDetails);
            var response = await cli.PostAsync(cli.BaseAddress + "api/PersonalDetails/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView","Home", new { UserId = personalDetails.UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = personalDetails.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(UserAdressTbl addressDetails)
        {
            HttpClient cli = _jobPortalUrl.initial();
            StringContent content = Serialization(addressDetails);
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserAdress/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = addressDetails.UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = addressDetails.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> EditEducation(UserEducationTbl educationDetails)
        {
            HttpClient cli = _jobPortalUrl.initial();            
            StringContent content = Serialization(educationDetails);
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserEducation/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = educationDetails.UserId});
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = educationDetails.UserId});
        }

        [HttpPost]
        public async Task<IActionResult> EditWork(UserWorkTbl UserWorkTbl)
        {
            HttpClient cli = _jobPortalUrl.initial();
            StringContent content = Serialization(UserWorkTbl);
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserWork/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> EditFollowing(int rowId,int UserId)
        {
            HttpClient cli = _jobPortalUrl.initial();            
            var response = await cli.DeleteAsync(cli.BaseAddress + "api/FollowersAndFollowing/" + rowId);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = UserId });            
        }

        [HttpGet]
        public async Task<IActionResult> EditFollowers(int rowId, int UserId)
        {
            HttpClient cli = _jobPortalUrl.initial();
            var response = await cli.DeleteAsync(cli.BaseAddress + "api/FollowersAndFollowing/" + rowId);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = UserId });
        }

        [HttpPost]
        public async Task<IActionResult> EditArticle(UserWorkTbl UserWorkTbl)
        {
            HttpClient cli = _jobPortalUrl.initial();
            StringContent content = Serialization(UserWorkTbl);
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserWork/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> EditArticlePinned(UserWorkTbl UserWorkTbl)
        {
            HttpClient cli = _jobPortalUrl.initial();
            StringContent content = Serialization(UserWorkTbl);
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserWork/Put", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
            }
            return RedirectToAction("ParticularView", "Home", new { UserId = UserWorkTbl.UserId });
        }


    }
}
