using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models
{
    public class Marker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Lng { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Source")]

        public int CitationId { get; set; }
        public Citation Citation { get; set; }

        [Required]
        [Display(Name = "Image")]

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
