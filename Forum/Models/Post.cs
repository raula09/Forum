using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }

        
        public ICollection<Comment> Comments { get; set; }
        public ICollection<GroupComment> CommentsGroup { get; set; }
        public ICollection<GroupComment> GroupComments { get; set; }
    }

}
