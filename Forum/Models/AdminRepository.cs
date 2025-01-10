using Forum.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class AdminRepository
    {
        private ForumDbContext _context;
        public AdminRepository(ForumDbContext context)
        {
            _context = context;
        }
        public void ViewAllPost()
        {
            try
            {
                var posts = _context.Posts.ToList();
                foreach (var item in posts)
                {
                    Console.WriteLine(item.Title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"no posts found: {ex.Message}");
            }
        }
        public void ViewAllUser()
        {
            try
            {
                var users = _context.Users.ToList();
                foreach (var item in users)
                {
                    Console.WriteLine($"Id:{item.Id} Username:{item.Username}");
                }
            }
            catch (Exception ex) {

                Console.WriteLine($"no users found");
            }
        }
        public void RemovePost(int postId)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
                if (post != null)
                {
                    
                    post.GroupId = null;

                    _context.Posts.Remove(post);
                    _context.SaveChanges();
                    Console.WriteLine("Post removed");
                }
                else
                {
                    Console.WriteLine("Couldn't find the post");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't remove post: {ex.Message}");
            }
        }


        public void RemoveUser(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
              
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    Console.WriteLine("user removed");
                }
                else
                {
                    Console.WriteLine("couldnt find user");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"couldnt remove user: {ex.Message}");
            }
        }

        public void ViewAllComments()
        {
            try
            {
                var comments = _context.Comments.Include(c => c.User).ToList();
                if (comments.Count > 0)
                {
                    foreach (var comment in comments)
                    {
                        Console.WriteLine($"{comment.User.Username}: \n {comment.Text} \n posted on {comment.CreatedAt}");
                    }
                }
                else
                {
                    Console.WriteLine("no comments");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"couldnt get all comments: {ex.Message}");
            }
        }

        public void RemoveGroup(int groupId)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);
                if (group != null)
                {
                    _context.Groups.Remove(group);
                    _context.SaveChanges();
                    Console.WriteLine("group removed");
                }
                else
                {
                    Console.WriteLine("couldnt find group");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"couldnt remove group: {ex.Message}");
            }
        }

        public void GiveUserAdminRole(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    user.Role = "Admin";
                    _context.SaveChanges();
                    Console.WriteLine($"{user.Username} is now admin");
                }
                else
                {
                    Console.WriteLine("user not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }

    }
}

