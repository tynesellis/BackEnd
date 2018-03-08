using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HulduNashville.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public virtual ICollection<Marker> Markers { get; set; }
    }
}