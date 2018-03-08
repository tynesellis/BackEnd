using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HulduNashville.Models
{
    public class Citation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        public virtual ICollection<Marker> Markers { get; set; }
    }
}