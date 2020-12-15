using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibespace.DATA;

namespace VibeSpace.MODELS
{
    public class UserInfoCreate
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public ICollection<Tag> Interests { get; set; }
    }
}
