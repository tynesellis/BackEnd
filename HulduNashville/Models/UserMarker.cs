using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models
{
    public class UserMarker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int MarkerId { get; set; }
        public Marker Marker { get; set; }

    }
}
