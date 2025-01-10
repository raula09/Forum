using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupComment> GroupComments { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }

}
