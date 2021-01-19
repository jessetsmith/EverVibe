using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibespace.DATA;

namespace VibeSpace.MODELS
{
    public class VibeDetail
    {
        public int VibeID { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<CommentsAndReactions> Comments { get; set; }
    }
}
