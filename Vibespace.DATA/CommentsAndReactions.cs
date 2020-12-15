using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeSpace.DATA;

namespace Vibespace.DATA
{
    public class CommentsAndReactions
    {
        [Key]
        public int CommentID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual ApplicationUser User { get; set; }
        public virtual string UserID { get; set; }
        [ForeignKey(nameof(VibeID))]
        public virtual Vibe Vibe { get; set; }
        public virtual int? VibeID { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}
