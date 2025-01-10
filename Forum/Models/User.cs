using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        
        public ICollection<GroupComment> GroupComments { get; set; }
    }
}
