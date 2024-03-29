﻿using JobPortalMVC.Models;
using JopPortalMVC.Helper;
using JopPortalMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using X.PagedList;

namespace JobPortalMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private IJobPortalUrl _jobPortalUrl;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _jobPortalUrl = new JobPortalUrl();
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginTbl login)
        {
            HttpClient cli = _jobPortalUrl.initial();

            StringContent content = Serialization(login);
            var response = await cli.PostAsync(cli.BaseAddress + "api/LoginTbls", content);

            if (response.IsSuccessStatusCode)
            {
                var StatusData = response.Content.ReadAsStringAsync().Result;
                
                JObject result = JObject.Parse(StatusData);
                var tokenData = result["token"].Value<string>();                
                HttpContext.Session.SetString("JWToken", tokenData);
                
                if (result.ContainsKey("info"))
                {
                    var clientarray = result["info"].Value<string>();
                    if (clientarray == "PersonalDetailsNotFilled")
                    {
                        var Oblejctarray = result["personalData"].ToObject<PersonalDetailsTbl>();
                        Console.WriteLine(Oblejctarray);
                        return RedirectToAction("personaldetailsnotfilled", Oblejctarray);
                    }
                    else if (clientarray == "WorkDetailsNotFilled")
                    {
                        return RedirectToAction("WorkDetailsNotFilled");
                    }
                    else if (clientarray == "EducationDetailsNotFilled")
                    {
                        return RedirectToAction("EducationDetailsNotFilled");
                    }
                }             
                    return RedirectToAction("deatilsview");
            }
            ViewBag.Error = "***  Invalid Valid User or Password  ***";
            return View();
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpData Data)
        {
            HttpClient cli = _jobPortalUrl.initial();
            string data;
            StringContent content = Serialization(Data);
            var response = await cli.PostAsync(cli.BaseAddress + "api/LoginTbls/SignUp", content);
            if (response.IsSuccessStatusCode)
            {
                var StatusData = response.Content.ReadAsStringAsync().Result;

                JObject result = JObject.Parse(StatusData);
                var tokenData = result["token"].Value<string>();
                HttpContext.Session.SetString("JWToken", tokenData);
                
                
                TempData["EmailRowId"] = result["emailRowId"].Value<string>();
                TempData["EmailId"] = result["emailId"].Value<string>();
                TempData["UserId"] = result["userId"].Value<string>();

                return RedirectToAction("SignUpPostData");
            }
            return View();
        }

        public string TempDataUserValue()
        {
            string EmailRowId = "";
            string EmailId = "";
            string UserId = "";


            if (TempData.ContainsKey("EmailRowId"))
            {
                EmailRowId = TempData["EmailRowId"].ToString();                
            }
            if (TempData.ContainsKey("EmailId"))
            {
                EmailId = TempData["EmailId"].ToString();
            }
            if (TempData.ContainsKey("UserId"))
            {
                UserId = TempData["UserId"].ToString();
            }


            return UserId+"|"+ EmailRowId +"|"+EmailId ;
        }
        public IActionResult SignUpPostData()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUpPostData(UserSignUpDataAll UserData)
        {
            UserProfilePhotoTbl Data = new UserProfilePhotoTbl();
            PersonalDetailsTbl PersonalData = new PersonalDetailsTbl();
            UserAdressTbl addressData = new UserAdressTbl();

            string SkillData = string.Join(",", UserData.Skills);
            PersonalData.Skills = SkillData;

            HttpClient cli = TokenValue();
            var authors = TempDataUserValue();
            string[] authorsList = authors.Split("|");
            UserData.UserId = Int32.Parse(authorsList[0]);
            PersonalData.RowId = Int32.Parse(authorsList[1]);
            PersonalData.Email = authorsList[2];

           
            Console.WriteLine(UserData.UserId);
            
                if (UserData.photo != null)
                {
                    string extension = Path.GetExtension(UserData.photo.FileName);
                    string CurrentDateTime = DateTime.Now.ToString();
                    CurrentDateTime = CurrentDateTime.Replace(" ", "_").Replace("/", "_").Replace(":", "_");
                    UserData.PhotoPath = "/Image/" + UserData.UserId+"_"+ UserData.FirstName +"_"+ CurrentDateTime + extension;
                    
                    Data.UserId = UserData.UserId;
                    Data.PhotoPath = UserData.PhotoPath;
                    StringContent content = Serialization(Data);
                    var response = await cli.PostAsync(cli.BaseAddress + "api/UserProfilePhoto", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string path = Path.Combine(wwwRootPath + UserData.PhotoPath);

                        await UserData.photo.CopyToAsync(new FileStream(path, FileMode.Create));
                    }
                    PersonalData.UserId = UserData.UserId;
                    PersonalData.FirstName = UserData.FirstName;
                    PersonalData.LastName = UserData.LastName;
                    PersonalData.PhoneNumber = UserData.PhoneNumber;
                    PersonalData.Gender = UserData.Gender;
                    PersonalData.DateOfBirth = UserData.DateOfBirth;
                    PersonalData.Experience = UserData.Experience;
                    
                    StringContent Personalcontent = Serialization(PersonalData);
                    var Personalresponse = await cli.PostAsync(cli.BaseAddress + "api/PersonalDetails/Put", Personalcontent);
                    if (Personalresponse.IsSuccessStatusCode)
                    {
                    Console.WriteLine("worked");
                    }
                    addressData.UserId = UserData.UserId;
                    addressData.PermantentDoorNumber = UserData.PermantentDoorNumber;
                    addressData.PermantentStreetName = UserData.PermantentStreetName;
                    addressData.PermantentArea = UserData.PermantentArea;
                    addressData.PermantentCity = UserData.PermantentCity;
                    addressData.PermantentPinCode = UserData.PermantentPinCode;
                    addressData.CurrentDoorNumber = UserData.CurrentDoorNumber;
                    addressData.CurrentStreetName = UserData.CurrentStreetName;
                    addressData.CurrentArea = UserData.CurrentArea;
                    addressData.CurrentCity = UserData.CurrentCity;
                    addressData.CurrentPinCode = UserData.CurrentPinCode;
                    StringContent addressDataContent = Serialization(addressData);
                    var AddressDetailsResponce= await cli.PostAsync(cli.BaseAddress + "api/UserAdress", addressDataContent);
                    if (AddressDetailsResponce.IsSuccessStatusCode)
                    {
                        return RedirectToAction("UserEducationAndWorkPost");
                    }
                }
            return View();
        }

        public IActionResult PersonalDetailsNotFilled(PersonalDetailsTbl UserDetails)
        {
            TempData["UserId"] = UserDetails.UserId;
            TempData["UserEmail"] = UserDetails.Email;
            TempData["PersonalRowId"] = UserDetails.RowId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDetailsNotFilled(UserSignUpDataAll UserData)
        {
            UserProfilePhotoTbl Data = new UserProfilePhotoTbl();
            PersonalDetailsTbl PersonalData = new PersonalDetailsTbl();
            UserAdressTbl addressData = new UserAdressTbl();

            string SkillData = string.Join(",", UserData.Skills);
            PersonalData.Skills = SkillData;

            HttpClient cli = TokenValue();
            if (TempData.ContainsKey("UserId"))
            {
                UserData.UserId = Convert.ToInt32(TempData["UserId"]);
            };
            if (TempData.ContainsKey("PersonalRowId"))
            {
                PersonalData.RowId = Convert.ToInt32(TempData["PersonalRowId"]);
            };
            if (TempData.ContainsKey("UserEmail"))
            {
                PersonalData.Email = Convert.ToString(TempData["UserEmail"]);
            }
            Console.WriteLine(UserData.UserId);
            //if (ModelState.IsValid)
            //{
            if (UserData.photo != null)
            {
                string extension = Path.GetExtension(UserData.photo.FileName);
                string CurrentDateTime = DateTime.Now.ToString();
                CurrentDateTime = CurrentDateTime.Replace(" ", "_").Replace("/", "_").Replace(":", "_");
                UserData.PhotoPath = "/Image/" + UserData.UserId + "_" + UserData.FirstName + "_" + CurrentDateTime + extension;

                Data.UserId = UserData.UserId;
                Data.PhotoPath = UserData.PhotoPath;
                StringContent content = Serialization(Data);
                var response = await cli.PostAsync(cli.BaseAddress + "api/UserProfilePhoto", content);
                if (response.IsSuccessStatusCode)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string path = Path.Combine(wwwRootPath + UserData.PhotoPath);

                    await UserData.photo.CopyToAsync(new FileStream(path, FileMode.Create));
                }
                PersonalData.UserId = UserData.UserId;
                PersonalData.FirstName = UserData.FirstName;
                PersonalData.LastName = UserData.LastName;
                PersonalData.PhoneNumber = UserData.PhoneNumber;
                PersonalData.Gender = UserData.Gender;
                PersonalData.DateOfBirth = UserData.DateOfBirth;
                PersonalData.Experience = UserData.Experience;
                //string SkillData = string.Join(",", UserData.Skills);
                //PersonalData.Skills = SkillData;
                StringContent Personalcontent = Serialization(PersonalData);
                var Personalresponse = await cli.PostAsync(cli.BaseAddress + "api/PersonalDetails/Put", Personalcontent);
                if (Personalresponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("worked");
                }
                addressData.UserId = UserData.UserId;
                addressData.PermantentDoorNumber = UserData.PermantentDoorNumber;
                addressData.PermantentStreetName = UserData.PermantentStreetName;
                addressData.PermantentArea = UserData.PermantentArea;
                addressData.PermantentCity = UserData.PermantentCity;
                addressData.PermantentPinCode = UserData.PermantentPinCode;
                addressData.CurrentDoorNumber = UserData.CurrentDoorNumber;
                addressData.CurrentStreetName = UserData.CurrentStreetName;
                addressData.CurrentArea = UserData.CurrentArea;
                addressData.CurrentCity = UserData.CurrentCity;
                addressData.CurrentPinCode = UserData.CurrentPinCode;
                StringContent addressDataContent = Serialization(addressData);
                var AddressDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/UserAdress", addressDataContent);
                if (AddressDetailsResponce.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        public IActionResult UserEducationAndWorkPost()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserEducationAndWorkPost(IFormCollection educationAndWork)
        {
            HttpClient cli = TokenValue();
            UserEducationTbl usereducation = new UserEducationTbl();
            
            var d1 = educationAndWork["ListId"].Count();
            var authors = TempDataUserValue();
            string[] authorsList = authors.Split("|");

            usereducation.UserId= Int32.Parse(authorsList[0]);
            

            for (int i =0;i< educationAndWork["ListId"].Count(); i++)
            {
                usereducation.EducationType= educationAndWork["EducationType"][i];
                usereducation.GroupName= educationAndWork["GroupName"][i];
                usereducation.CompletedEducationIn= educationAndWork["CompletedEducationIn"][i];
                usereducation.YearOfStart= Int32.Parse(educationAndWork["YearOfStart"][i]);
                usereducation.YearOfEnd= Int32.Parse(educationAndWork["YearOfEnd"][i]);
                usereducation.PercentageObtained= decimal.Parse(educationAndWork["PercentageObtained"][i]);
                StringContent EducationContent = Serialization(usereducation);
                var AddressDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/UserEducation", EducationContent);
                if (!AddressDetailsResponce.IsSuccessStatusCode)
                {
                    return View();
                }
            }
            return RedirectToAction("WorkPost");
        }
        public IActionResult WorkPost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WorkPost(IFormCollection Work)
        {
            HttpClient cli = TokenValue();
            UserWorkTbl userWork = new UserWorkTbl();
            
            var authors = TempDataUserValue();
            string[] authorsList = authors.Split("|");
            userWork.UserId = Int32.Parse(authorsList[0]);

            for (int i = 0; i < Work["WorkId"].Count(); i++)
            {
                userWork.CompanyName = Work["CompanyName"][i];
                userWork.WorkedFrom = Int32.Parse(Work["WorkedFrom"][i]);
                userWork.WorkedTill = Int32.Parse(Work["WorkedTill"][i]);
                StringContent WorkContent = Serialization(userWork);
                var WorkDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/UserWork", WorkContent);
                if (!WorkDetailsResponce.IsSuccessStatusCode)
                {
                    return View();
                }
            }

            return RedirectToAction("DeatilsView");
        }
        public IActionResult EducationDetailsNotFilled()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EducationDetailsNotFilled(IFormCollection educationAndWork)
        {
            HttpClient cli = TokenValue();
            UserEducationTbl usereducation = new UserEducationTbl();

            var d1 = educationAndWork["ListId"].Count();
            //var authors = TempDataUserValue();
            //string[] authorsList = authors.Split("|");

            usereducation.UserId = Int32.Parse(UserClaim());


            for (int i = 0; i < educationAndWork["EducationType"].Count(); i++)
            {
                usereducation.EducationType = educationAndWork["EducationType"][i];
                usereducation.GroupName = educationAndWork["GroupName"][i];
                usereducation.CompletedEducationIn = educationAndWork["CompletedEducationIn"][i];
                usereducation.YearOfStart = Int32.Parse(educationAndWork["YearOfStart"][i]);
                usereducation.YearOfEnd = Int32.Parse(educationAndWork["YearOfEnd"][i]);
                usereducation.PercentageObtained = decimal.Parse(educationAndWork["PercentageObtained"][i]);
                StringContent EducationContent = Serialization(usereducation);
                var AddressDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/UserEducation", EducationContent);
                if (!AddressDetailsResponce.IsSuccessStatusCode)
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        public IActionResult WorkDetailsNotFilled()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WorkDetailsNotFilled(IFormCollection Work)
        {
            HttpClient cli = TokenValue();
            UserWorkTbl userWork = new UserWorkTbl();

            var authors = TempDataUserValue();
            //string[] authorsList = authors.Split("|");
            userWork.UserId = Int32.Parse(UserClaim());

            for (int i = 0; i < Work["CompanyName"].Count(); i++)
            {
                userWork.CompanyName = Work["CompanyName"][i];
                userWork.WorkedFrom = Int32.Parse(Work["WorkedFrom"][i]);
                userWork.WorkedTill = Int32.Parse(Work["WorkedTill"][i]);
                StringContent WorkContent = Serialization(userWork);
                var WorkDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/UserWork", WorkContent);
                if (!WorkDetailsResponce.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("DeatilsView");
        }

        [HttpGet]
        public async Task<IActionResult> DeatilsView(String sort_order, string searchString, string gender, String skill, int? page)
        {
            HttpClient cli = TokenValue();
            List<SelectListItem> Skill = new List<SelectListItem>();
            List<SelectListItem> Gender = new List<SelectListItem>();
            List<SelectListItem> Sort = new List<SelectListItem>();

            ViewData["ValueFilter"] = searchString;

            Sort.Add(new SelectListItem { Text = "Select", Value = "0" });
            Sort.Add(new SelectListItem { Text = "Email", Value = "Email_desc" });
            Sort.Add(new SelectListItem { Text = "Experience", Value = "Experience_desc" });
            Sort.Add(new SelectListItem { Text = "Skills", Value = "Skills_desc" });
            TempData["Sort"] = Sort;

            Skill.Add(new SelectListItem { Text = "Select", Value = "0"});
            foreach (string item in Enum.GetNames(typeof(Skills)))
            {
                Skill.Add(new SelectListItem { Text = item, Value = item });
            }
            TempData["Skill"] = Skill;

            Gender.Add(new SelectListItem { Text = "Select", Value = "0" });
            Gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            Gender.Add(new SelectListItem { Text = "FeMale", Value = "FeMale" });
            TempData["Gender"] = Gender;

            
            List<DetailsView> AllDetailsViewData = new List<DetailsView>();

            HttpResponseMessage AllDetails = await cli.GetAsync("api/AllDetailsView");
            if (AllDetails.IsSuccessStatusCode)
            {
                var DetailsRes = AllDetails.Content.ReadAsStringAsync().Result;
                AllDetailsViewData = JsonConvert.DeserializeObject<List<DetailsView>>(DetailsRes);
            }
            if (gender == "Male" || gender == "FeMale")
            {
                AllDetailsViewData = AllDetailsViewData.Where(c => c.Gender.Contains(gender)).ToList();
            }
            switch (skill)
            {
                case "DotNet":
                    AllDetailsViewData = AllDetailsViewData.Where(s => s.Skills.Contains("Dot Net")).ToList();
                    break;
                case "Python":
                    AllDetailsViewData = AllDetailsViewData.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
                case "Angular":
                    AllDetailsViewData = AllDetailsViewData.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
                case "React":
                    AllDetailsViewData = AllDetailsViewData.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
                case "MachineLeanrning":
                    AllDetailsViewData = AllDetailsViewData.Where(s => s.Skills.Contains("Machine Leanrning")).ToList();
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                AllDetailsViewData = AllDetailsViewData.Where(s => s.FullName.ToLower().Any(value =>
                                       s.Gender.ToLower().Any(value => s.Gender.ToLower().Contains(searchString)) || 
                                       s.FullName.ToLower().Contains(searchString)) ||                                       
                                       s.Email.ToLower().Any(value => s.Email.ToLower().Contains(searchString)) ||                                       
                                       s.Skills.ToLower().Any(value => s.Skills.ToLower().Contains(searchString)) ||                                       
                                       s.Experience.ToString().Contains(searchString)).ToList();
            }
            switch (sort_order)
            {
                case "Email_desc":
                    AllDetailsViewData = AllDetailsViewData.OrderByDescending(s => s.Email).ToList();
                    break;
                case "Experience_desc":
                    AllDetailsViewData = AllDetailsViewData.OrderByDescending(s => s.Experience).ToList();
                    break;
                case "Skills_desc":
                    AllDetailsViewData = AllDetailsViewData.OrderByDescending(s => s.Skills).ToList();
                    break;
                default:
                    AllDetailsViewData = AllDetailsViewData.OrderBy(s => s.Experience).ToList();
                    break;
            }
            var pageNumber = page ?? 1;
            return View(AllDetailsViewData.ToList().ToPagedList(pageNumber, 1));
        }

        public async Task<IActionResult> ParticularView(int UserId)
        {            
            ViewBag.UserId = UserClaim();

            HttpClient cli = TokenValue();
            List<UserProfilePhotoTbl> PhotoData = new List<UserProfilePhotoTbl>();
            List<PersonalDetailsTbl> PersonalData = new List<PersonalDetailsTbl>();
            List<UserAdressTbl> AddressData = new List<UserAdressTbl>();
            List<ArticleTbl> ArticleData = new List<ArticleTbl>();
            List<ArticlePinedTbl> ArticlelPinedData = new List<ArticlePinedTbl>();
            List<UserEducationTbl> EducationData = new List<UserEducationTbl>();
            List<FollowingFollowerNameDetails> FollowersData = new List<FollowingFollowerNameDetails>();
            List<FollowingFollowerNameDetails> FollowingData = new List<FollowingFollowerNameDetails>();
            List<UserWorkTbl> WorkData = new List<UserWorkTbl>();
            
            List<SortedProfileList> SortedResultdata = new List<SortedProfileList>();


            HttpResponseMessage PhotoDetails = await cli.GetAsync("api/UserProfilePhoto/" + UserId);
            if (PhotoDetails.IsSuccessStatusCode)
            {
                var PhotoRes = PhotoDetails.Content.ReadAsStringAsync().Result;
                PhotoData = JsonConvert.DeserializeObject<List<UserProfilePhotoTbl>>(PhotoRes);
            }
            HttpResponseMessage PersonalDetails = await cli.GetAsync("api/PersonalDetails/" + UserId);
            if (PersonalDetails.IsSuccessStatusCode)
            {
                var PersonalRes = PersonalDetails.Content.ReadAsStringAsync().Result;
                PersonalData = JsonConvert.DeserializeObject<List<PersonalDetailsTbl>>(PersonalRes);
            }
            HttpResponseMessage AddressDetails = await cli.GetAsync("api/UserAdress/" + UserId);
            if (AddressDetails.IsSuccessStatusCode)
            {
                var AddressRes = AddressDetails.Content.ReadAsStringAsync().Result;
                AddressData = JsonConvert.DeserializeObject<List<UserAdressTbl>>(AddressRes);
            }
            HttpResponseMessage ArticleDetails = await cli.GetAsync("api/ArticleAndTitle/" + UserId);
            if (ArticleDetails.IsSuccessStatusCode)
            {
                var ArticleRes = ArticleDetails.Content.ReadAsStringAsync().Result;
                ArticleData = JsonConvert.DeserializeObject<List<ArticleTbl>>(ArticleRes);
            }
            HttpResponseMessage ArticlelPinedDetails = await cli.GetAsync("api/ArticlePined/" + UserId);
            if (ArticleDetails.IsSuccessStatusCode)
            {
                var ArticlelPinedRes = ArticlelPinedDetails.Content.ReadAsStringAsync().Result;
                ArticlelPinedData = JsonConvert.DeserializeObject<List<ArticlePinedTbl>>(ArticlelPinedRes);
            }
            HttpResponseMessage EducationDetails = await cli.GetAsync("api/UserEducation/" + UserId);
            if (EducationDetails.IsSuccessStatusCode)
            {
                var EducationRes = EducationDetails.Content.ReadAsStringAsync().Result;
                EducationData = JsonConvert.DeserializeObject<List<UserEducationTbl>>(EducationRes);
            }
            HttpResponseMessage FollowersDetails = await cli.GetAsync("api/FollowersAndFollowing/" + UserId);
            if (FollowersDetails.IsSuccessStatusCode)
            {
                var FollowersRes = FollowersDetails.Content.ReadAsStringAsync().Result;
                FollowersData = JsonConvert.DeserializeObject<List<FollowingFollowerNameDetails>>(FollowersRes);

                //getting the user following details
                FollowingData = FollowersData.Where(c => c.FollowerId == UserId).ToList();
                //getting the user followers details
                FollowersData = FollowersData.Where(c => c.UserId == UserId).ToList();
            }
            HttpResponseMessage WorkDetails = await cli.GetAsync("api/UserWork/" + UserId);
            if (WorkDetails.IsSuccessStatusCode)
            {
                var WorkRes = WorkDetails.Content.ReadAsStringAsync().Result;
                WorkData = JsonConvert.DeserializeObject<List<UserWorkTbl>>(WorkRes);
            }

            ViewBag.SortedProfile = PhotoData[0].UserId;
            Console.WriteLine(ViewBag.SortedProfile);
            ViewModelData ViewData = new ViewModelData
            {
                UserProfilePhotoTbl = PhotoData,
                PersonalDetailsTbl = PersonalData,
                AddressDetailsTbl = AddressData,
                UserEducationTbl = EducationData,
                UserWorkTbl = WorkData,
                UserFollowingDetails = FollowingData,
                UserFollowersDetails = FollowersData,
                ArticleTbl =  ArticleData,
                ArticlePinedTbl =ArticlelPinedData
            };
            return View(ViewData);
        }
        
        

        [HttpPost]
        public async Task<IActionResult> EditPersonal(PersonalDetailsTbl details)
        {
            HttpClient cli = TokenValue();

            StringContent PersonalContent = Serialization(details);
            var WorkDetailsResponce = await cli.PostAsync(cli.BaseAddress + "api/PersonalDetails/Put", PersonalContent);            
            if (WorkDetailsResponce.IsSuccessStatusCode)
            {
                return RedirectToAction("ParticularView",details.UserId);
            }
            return RedirectToAction("ParticularView", details.UserId);
        }

        [HttpGet]
        public IActionResult ArticleCreate()
        {
            return View();
        }

        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<IActionResult> ArticleCreate(ArticleTbl article)
        {
            HttpClient cli = TokenValue();
            article.UserId = Int32.Parse(UserClaim());
            
            var Title = article.Title.Split('#').Select(p => p).ToList();
            Title.RemoveAt(0);
            Console.WriteLine(Title);


            StringContent content = Serialization(article);
            var response = await cli.PostAsync(cli.BaseAddress + "api/ArticleAndTitle/ArticlePost", content);
            if (response.IsSuccessStatusCode)
            {
                var value =  response.Content.ReadAsStringAsync().Result;
                ArticleTitleTbl ArticleTitleDetails = new ArticleTitleTbl();
                ArticleTitleDetails.ArticleId = Int32.Parse(value);                
                Console.WriteLine(value);
                foreach (var ArticleTitle in Title)
                {
                    ArticleTitleDetails.Title = ArticleTitle;
                    StringContent Titlecontent = Serialization(ArticleTitleDetails);
                    var Titleresponse = await cli.PostAsync(cli.BaseAddress + "api/ArticleAndTitle/TitlePost", Titlecontent);
                    if (Titleresponse.IsSuccessStatusCode)
                    {
                        continue;
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewArticle(int userId)
        {
            List<ArticleTbl> ParticularArticleDetails = new List<ArticleTbl>();

            HttpClient cli = TokenValue();

            var ParticularArticleResult = await cli.GetAsync(cli.BaseAddress + "api/ArticleAndTitle/" + userId);
            if (ParticularArticleResult.IsSuccessStatusCode)
            {
                var RawArticleResult = ParticularArticleResult.Content.ReadAsStringAsync().Result;
                ParticularArticleDetails = JsonConvert.DeserializeObject<List<ArticleTbl>>(RawArticleResult);
                return View(ParticularArticleDetails);
            }
            return RedirectToAction("DeatilsView");
        }

        public async Task<IActionResult> ListArticle(string searchString)
        {
            ViewData["ValueFilter"] = searchString;
            HttpClient cli = TokenValue();
            List<ArticleList> ArticleDetails = new List<ArticleList>();
            List<ArticleList> FinalArticleDetails = new List<ArticleList>();
            var ArticleResponse = await cli.GetAsync("api/ArticleAndTitle");
            if (ArticleResponse.IsSuccessStatusCode)
            {
                var ArtileResult = ArticleResponse.Content.ReadAsStringAsync().Result;
                ArticleDetails = JsonConvert.DeserializeObject<List<ArticleList>>(ArtileResult);
               
            }
            if (!String.IsNullOrEmpty(searchString))
            {               
                
                var Title = searchString.Split(',').Select(p => p.ToLower());

                foreach (var item in Title)
                {
                    FinalArticleDetails = ArticleDetails.Where(s => s.Category.ToLower().Any(value => s.Category.ToLower().Contains(item)) ||
                                                                    s.Title.ToLower().Any(value => s.Title.ToLower().Contains(item)) ||
                                                                    s.Skills.ToLower().Any(value => s.Skills.ToLower().Contains(item)) ||
                                                                    s.Name.ToLower().Any(value => s.Name.ToLower().Contains(item)) ||
                                                                    s.Email.ToLower().Any(value => s.Email.ToLower().Contains(item))).ToList();                                                                    
                                                                    
                }
                FinalArticleDetails = FinalArticleDetails.GroupBy(x => x.RowId).Select(g => new ArticleList { RowId = g.Key,UserId = g.FirstOrDefault().UserId,Title=g.FirstOrDefault().Title,
                                                                                                Category = g.FirstOrDefault().Category, Content = g.FirstOrDefault().Content,
                                                                                                   Name = g.FirstOrDefault().Name,Skills = g.FirstOrDefault().Skills,Email = g.FirstOrDefault().Email}).ToList();
                return View(FinalArticleDetails);
            }
            return View(ArticleDetails);
        }

        public async Task<IActionResult> SortedView()
        {
            HttpClient cli = TokenValue();
            int SelectedBy = Int32.Parse(UserClaim());
            List<SortedProfileList> SortedResultdata = new List<SortedProfileList>();
            var SortedResponse = await cli.GetAsync("api/SortedProfiles/" + SelectedBy);
            if (SortedResponse.IsSuccessStatusCode)
            {
                var SortedResult = SortedResponse.Content.ReadAsStringAsync().Result;
                SortedResultdata = JsonConvert.DeserializeObject<List<SortedProfileList>>(SortedResult);

            }
            return View(SortedResultdata);
        }

        public async Task<IActionResult> RemoveSort(int RowId)
        {
            HttpClient cli = TokenValue();
            int SelectedBy = Int32.Parse(UserClaim());            
            var SortedResponse = await cli.DeleteAsync("api/SortedProfiles/" + RowId);
            if (SortedResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("SortedView");
            }
            return RedirectToAction("SortedView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
