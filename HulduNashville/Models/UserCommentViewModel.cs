using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HulduNashville.Models
{
    public class UserCommentViewModel
    {
        public int CommentId { get; set; }

        public string CommentString { get; set; }

        public string UserName { get; set; }

        public int MarkerId { get; set; }
    }
}
