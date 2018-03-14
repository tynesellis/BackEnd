using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CommentText { get; set; }

        [Required]
        public int UserMarkerId { get; set; }
        public UserMarker UserMarker { get; set; }

        public virtual ICollection<UserMarker> UserMarkers { get; set; }
    }
}
