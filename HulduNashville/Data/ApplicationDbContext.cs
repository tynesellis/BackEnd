using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HulduNashville.Models;

namespace HulduNashville.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<HulduNashville.Models.Image> Image { get; set; }

        public DbSet<HulduNashville.Models.Citation> Citation { get; set; }

        public DbSet<HulduNashville.Models.Category> Category { get; set; }

        public DbSet<HulduNashville.Models.Marker> Marker { get; set; }

        public DbSet<HulduNashville.Models.UserRole> UserRole { get; set; }

        public DbSet<HulduNashville.Models.Comment> Comment { get; set; }
    }
}
