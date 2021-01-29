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
        [ForeignKey(nameof(ApplicationUser))]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(Vibe))]
        public int VibeID { get; set; }
        public virtual Vibe Vibe { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}
