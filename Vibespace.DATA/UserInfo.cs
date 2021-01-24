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
    public class UserInfo
    {
        public UserInfo()
        {
            this.Interests = new HashSet<Tag>();
        }
        [Key]
        public int UserInfoID { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public byte[] ProfileImage { get; set; }
        public virtual ICollection<Tag> Interests { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}
