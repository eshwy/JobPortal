using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class DetailsView
    {
        public int UserId { get; set; }
        public string PhotoPath { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Experience { get; set; }
        public string Skills { get; set; }
    }

    public enum Skills
    {
        [Description("Dot Net")]
        DotNet,
        Python,
        Angular,
        React,
        [Description("Machine Leanrning")]
        MachineLearning,
        
    }
}
