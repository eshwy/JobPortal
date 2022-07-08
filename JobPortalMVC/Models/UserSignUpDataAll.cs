using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalMVC.Models
{
    public class UserSignUpDataAll
    {
        
        public int? UserId { get; set; }
        //Photo Details
        public string PhotoPath { get; set; }
        [Required]
        public IFormFile photo { get; set; }
        //personal details
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public int? Experience { get; set; }
        [Required]
        public string[] Skills { get; set; }
        //Adress Details
        [Required]
        public string CurrentDoorNumber { get; set; }
        [Required]
        public string CurrentStreetName { get; set; }
        [Required]
        public string CurrentArea { get; set; }
        [Required]
        public string CurrentCity { get; set; }
        [Required]
        public int? CurrentPinCode { get; set; }
        [Required]
        public string PermantentDoorNumber { get; set; }
        [Required]
        public string PermantentStreetName { get; set; }
        [Required]
        public string PermantentArea { get; set; }
        [Required]
        public string PermantentCity { get; set; }
        [Required]
        public int? PermantentPinCode { get; set; }        
        
    }
    public enum Gender
    {
        Male,
        Female
    }
    public enum Skill
    {
        [Description("Dot Net")]
        DotNet,
        Python,
        Angular,
        React,
        [Description("Machine Leanrning")]
        MachineLearning,
        DBMS
    }
}
