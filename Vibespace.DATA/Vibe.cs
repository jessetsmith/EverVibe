using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeSpace.DATA;

namespace Vibespace.DATA
{
    public class Vibe
    {
        [Key]
        public int VibeID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual ApplicationUser User { get; set; }
        public virtual string UserID { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<CommentsAndReactions> Comments { get; set; }
        public bool Private { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}
