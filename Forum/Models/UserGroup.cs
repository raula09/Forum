using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }

}
