using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HulduNashville.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;

namespace HulduNashville.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        [Display(Name = "User Name")]
        public string DisplayName { get; set; }

    }
}
