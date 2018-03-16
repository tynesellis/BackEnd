using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models.ManageViewModels
{
    public class AssignRoleViewModel
    {
        public SelectList Users { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }

        public string UserId { get; set; }

        public string RoleId { get; set; }
    }
}
