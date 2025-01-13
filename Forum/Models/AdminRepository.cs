using Forum.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Forum.Models
{
    public class AdminRepository
    {
        private readonly ForumDbContext _context;

        public AdminRepository(ForumDbContext context)
        {
            _context = context;
        }

        public void ViewAllPosts()
        {
            try
            {
                var posts = _context.Posts.Include(p => p.Comments).ToList();

                if (posts.Count == 0)
                {
                    Console.WriteLine("No posts found.");
                    return;
                }

                foreach (var post in posts)
                {
                    Console.WriteLine($"Post ID: {post.Id}, Title: {post.Title}, Comments: {post.Comments.Count}");
                    Console.WriteLine("_______________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving posts: {ex.Message}\n{ex.StackTrace}");
            }
        }
        public void ViewUserPosts(int userId)
        {
            try { 
                
                var user = _context.Users.Include(u => u.Posts).FirstOrDefault(p => p.Id == userId);

                if (user == null) 
                {
                    Console.WriteLine($"user with {user.Id} doesnt exist ");
                }
                if (user.Posts == null) 
                {
                    Console.WriteLine($"{user.Username} doesnt have any posts");
                }
            foreach (var post in user.Posts) {
                Console.WriteLine($"{post.Id} \n {post.Title} \n {post.Content} \n {post.CreatedAt} ");
                    Console.WriteLine("_______________________________________________________________________________");

                }
            }
           
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
        public void ViewUserComments(int userId) 
        {
            try
            {
                var user = _context.Users.Include(u =>u.Comments).FirstOrDefault(p => p.Id == userId);
                if (user == null) 
                {
                    Console.WriteLine($"user with {user.Id} doesnt exist");

                }
                if(user.Comments == null)
                {
                    Console.WriteLine($"{user.Username} doesnt have any Comments posted");
                }
                foreach (var comment in user.Comments)
                {
                    Console.WriteLine($"{comment.Id} \n {comment.Text} \n {comment.CreatedAt}");
                    Console.WriteLine("_______________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }

        }
        public void ViewAllUsers()
        {
            try
            {
                var users = _context.Users.Include(u => u.Comments).ToList();

                if (users.Count == 0)
                {
                    Console.WriteLine("No users found.");
                    return;
                }

                foreach (var user in users)
                {
                    Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}, Comments: {user.Comments.Count}");
                    Console.WriteLine("_______________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void RemovePost(int postId)
        {
            try
            {
                var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(x => x.Id == postId);
                if (post == null)
                {
                    Console.WriteLine("Post not found.");
                    return;
                }

                _context.Posts.Remove(post);
                _context.SaveChanges();
                Console.WriteLine("Post removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing post: {ex.Message}\n{ex.StackTrace}");
            }
        }
        public void RemoveComment(int commentId)
        {
            try 
            { 
             var comment = _context.Comments.FirstOrDefault(x => x.Id == commentId);
                if(comment == null)
                {
                    Console.WriteLine("comment not found");
                    return;
                }
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                Console.WriteLine("comment removed succesfully");
            
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            }

        public void RemoveUser(int userId)
        {
            try
            {
                var user = _context.Users.Include(u => u.Comments).Include(u => u.UserGroups).FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
                Console.WriteLine("User removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing user: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void ViewAllComments()
        {
            try
            {
                var comments = _context.Comments.Include(c => c.User).Include(c => c.Post).ToList();

                if (comments.Count == 0)
                {
                    Console.WriteLine("No comments found.");
                    return;
                }

                foreach (var comment in comments)
                {
                    Console.WriteLine($"{comment.User?.Username} commented on Post {comment.Post?.Title}: {comment.Text} (Posted on {comment.CreatedAt})");
                    Console.WriteLine("_______________________________________________________________________________");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving comments: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void RemoveGroup(int groupId)
        {
            try
            {
                var group = _context.Groups.Include(g => g.GroupComments).FirstOrDefault(x => x.Id == groupId);
                if (group == null)
                {
                    Console.WriteLine("Group not found.");
                    return;
                }

                _context.Groups.Remove(group);
                _context.SaveChanges();
                Console.WriteLine("Group removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing group: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public void GiveUserAdminRole(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                user.Role = "Admin";
                _context.SaveChanges();
                Console.WriteLine($"User {user.Username} is now an Admin.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user role: {ex.Message}\n{ex.StackTrace}");
            }
        }

    }
}
