using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibespace.DATA;

namespace VibeSpace.MODELS
{
    public class UserInfoDetail
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public byte[] ProfileImage { get; set; }
        public virtual ICollection<Tag> Interests { get; set; }
    }
}
