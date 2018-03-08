using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HulduNashville.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Marker> Markers { get; set; }
    }
}