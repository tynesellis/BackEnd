using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public IdentityRole Role { get; set; }

        public ApplicationUser User { get; set; }
    }
}
