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

        [Required]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }


        public virtual ICollection<Marker> Markers { get; set; }
    }
}