using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeSpace.MODELS
{
    public class CommentDetail
    {
        public string Username { get; set; }
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        [Display(Name = "Created:")]
        public DateTimeOffset DateCreated { get; set; }

    }
}
