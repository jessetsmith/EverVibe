using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibespace.DATA
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        public string TagName { get; set; }
    }
}
