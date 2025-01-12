using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Forum.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Forum.Models
{
    public class ForumRepository
    {
        private ForumDbContext _context;
        public ForumRepository(ForumDbContext context)
        {
            _context = context;
        }
        private void ViewAllPost()
        {
            try
            {
                var posts = _context.Posts
                                    .Include(p => p.User)  
                                    .ToList();

                if (posts != null)
                {
                    foreach (var post in posts)
                    {
                        Console.WriteLine($"post id: {post.Id} \n User: {post.User?.Username}\n Title: {post.Title}\n Content: {post.Content}\n posted on: {post.CreatedAt}");
                        Console.WriteLine("_______________________________________________________________________________");
                    }
                }
                else
                {
                    Console.WriteLine("No posts found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }


        public void AddPost(int userId)
        {
            try
            {
                Console.WriteLine("enter title");
                var title = Console.ReadLine();
                Console.WriteLine("enter content");
                var content = Console.ReadLine();

                Post post = new Post
                {
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    GroupId = null,
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
                Console.WriteLine("post added");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }


        public void ViewMyPosts(int userId)
        {
            try
            {
        
               
                var posts = _context.Posts.Where(p => p.UserId == userId).ToList();
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.Id} \n {post.Title} \n {post.Content} \n {post.CreatedAt} ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }

        public void DeleteMyPost(int postId, int userId)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
                if (post != null && post.UserId == userId)
                {
                    _context.Posts.Remove(post);
                    _context.SaveChanges();
                    Console.WriteLine("post deleted");
                }
                else
                {
                    Console.WriteLine("you are not the author or error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }

        public void AddCommentToPost(int postId, int userId, string commentText)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
                if (post == null)
                {
                    Console.WriteLine("couldnt find post");
                    return;
                }
                Comment comment = new Comment
                {
                    Text = commentText,
                    CreatedAt = DateTime.Now,
                    PostId = postId,
                    UserId = userId
                };
                _context.Comments.Add(comment);
                _context.SaveChanges();
                Console.WriteLine("comment posted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
     

      
        public void SearchPosts(string postName)
        {
            var posts = _context.Posts
                                .Where(p => p.Title.Contains(postName) || p.Content.Contains(postName))
                                .ToList();

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    Console.WriteLine($"Title:{post.Title}; \n Content:{post.Content}");
                }

            }
            else
            {
                Console.WriteLine("couldnt find post");
            }
        }

        public void CreateGroup(string groupName, int userId)
        {
            Group group = new Group
            {
                Name = groupName,

            };
            _context.Groups.Add(group);
            _context.SaveChanges();

            UserGroup userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = group.Id
            };
            _context.UserGroups.Add(userGroup);
            _context.SaveChanges();
            Console.WriteLine($"Group {group.Name} created");
        }
        public void JoinGroup(int groupId, int userId)
        {
            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (group != null && user != null)
            {
                UserGroup userGroup = new UserGroup
                {
                    UserId = userId,
                    GroupId = groupId
                };
                _context.UserGroups.Add(userGroup);
                _context.SaveChanges();
                Console.WriteLine("user added to group");
            }
            else
            {
                Console.WriteLine("Group or User not found");
            }
        }
        public void ViewGroupPosts(int groupId)
        {
            var posts = _context.Posts.Include(p => p.User).Where(g => g.GroupId == groupId).ToList();
            foreach (var post in posts)
            {
                Console.WriteLine($" Author:{post.User.Username}; \nTitle:{post.Title};\n  Content:{post.Content};\n Posted at:{post.CreatedAt};");
            }
        }
        public void AddPostToGroup(int groupId, int userId)
        {
            try
            {
                
                Console.WriteLine("Enter title for the new post:");
                var title = Console.ReadLine();

                Console.WriteLine("Enter content for the new post:");
                var content = Console.ReadLine();

              
                Post newPost = new Post
                {
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    GroupId = groupId
                };

                
                _context.Posts.Add(newPost);
                _context.SaveChanges();

                Console.WriteLine("Post successfully added to the group.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void AddCommentToGroupPost(int postId, int groupId, int userId, string commentText)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                Console.WriteLine("post not found");
            }
            Comment comment = new Comment
            {
                Text = commentText,
                UserId = userId,
                PostId = postId,

                CreatedAt = DateTime.Now,
            };
            _context.Add(comment);
            _context.SaveChanges();
            Console.WriteLine("Posted");
        }
        public void DisplayGroupComments(int groupId)
        {
            var groupPosts = _context.Posts.Where(p => p.Id == groupId).ToList();
            if (groupPosts.Count > 0)
            {
                Console.WriteLine("no posts in this group");
            }

            var comments = _context.Comments
                            .Where(c => groupPosts.Any(p => p.Id == c.PostId)).ToList();


            if (comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    Console.WriteLine($"comment by {comment.User.Username} \n  {comment.Text} \n posted on {comment.CreatedAt}");
                }
            }
            else
            {
                Console.WriteLine("no comment ");
            }

        }
        public void ViewPostComments(int postId)
        {
            var comments = _context.Comments.Where(p => p.PostId == postId).ToList();
            if (comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    Console.WriteLine($"comment by {comment.User.Username}  \n {comment.Text} \n posted in {comment.CreatedAt}");
                }
            }
        }
        public void EditPost(int postId, int userId, string newTitle, string newContent)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(p => p.Id == postId && p.UserId == userId);
                if (post != null)
                {
                    post.Title = newTitle;
                    post.Content = newContent;
                    _context.SaveChanges();
                    Console.WriteLine("Post updated successfully.");
                }
                else
                {
                    Console.WriteLine("Post not found or you are not the author.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ViewAllGroups()
        {
            try
            {
                var groups = _context.Groups.ToList();
                if (groups != null)
                {
                    foreach (var item in groups)
                    {
                        Console.WriteLine($"GroupID: {item.Id} Group Name: {item.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No groups found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void SharePostToGroup(int postId, int groupId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
            if (post != null && group != null)
            {
                post.GroupId = groupId;
                _context.SaveChanges();
                Console.WriteLine("Post shared to group");
            }
            else
            {
                Console.WriteLine("Error: Post or Group not found.");
            }
        }

        public void RegistrationMenu(UserManagement userManagement)
        {
            while (true)
            {
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                string option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            RegisterUser(userManagement);
                            break;

                        case "2":
                            LoginUser(userManagement);
                            break;

                        case "3":
                            Console.WriteLine(" Bye bye");
                            return;

                        default:
                            Console.WriteLine("try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        private void RegisterUser(UserManagement userManagement)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Role (admin/user): ");
            string role = Console.ReadLine().ToLower();

            if (role != "admin" && role != "user")
            {
                Console.WriteLine("try again");
                return;
            }

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

     
            userManagement.Register(name, password, role);



            User loggedInUser = userManagement.LoginUser(name, password);

            if (loggedInUser != null)
            {
                Console.WriteLine($"Welcome, {loggedInUser.Username}!");


                if (loggedInUser.Role.ToLower() == "admin")
                {
                    AdminMenu(loggedInUser);
                }
                else if (loggedInUser.Role.ToLower() == "user")
                {
                    UserMenu(loggedInUser);
                }
                else
                {
                    Console.WriteLine("error1");
                }
            }
            else
            {
                Console.WriteLine("error2.");
            }
        }

        private void LoginUser(UserManagement userManagement)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User loggedInUser = userManagement.LoginUser(name, password);

            if (loggedInUser == null)
            {
                Console.WriteLine("try again");
                return;
            }

            Console.WriteLine($"Welcome, {loggedInUser.Username}!");


            if (loggedInUser.Role.ToLower() == "admin")
            {
                AdminMenu(loggedInUser);
            }
            else if (loggedInUser.Role.ToLower() == "user")
            {
                UserMenu(loggedInUser);
            }
            else
            {
                Console.WriteLine("error");
            }

        }


        private void UserMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. View All Posts");
                Console.WriteLine("2. Add Post");
                Console.WriteLine("3. View My Posts");
                Console.WriteLine("4. Remove My Post");
                Console.WriteLine("5. Add Comment to Post");
                Console.WriteLine("6. Search Posts");
                Console.WriteLine("7. Create Group");
                Console.WriteLine("8. Join Group");
                Console.WriteLine("9. View Group Posts");
                Console.WriteLine("10. Add Post to Group");
                Console.WriteLine("11. Add Comment to Group Post");
                Console.WriteLine("12. Display Group Comments");
                Console.WriteLine("13. view post Comments");
                Console.WriteLine("14. view all Groups");
                Console.WriteLine("15. share post to group");
                Console.WriteLine("16. edit post");
                Console.WriteLine("17. Logout");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllPost();
                        break;

                    case "2":

                        AddPost(user.Id);
                        
                        break;

                    case "3":
                        ViewMyPosts(user.Id);
                        break;

                    case "4":
                        Console.WriteLine("Enter post id to delete:");
                        int postIdToDelete = int.Parse(Console.ReadLine());
                        DeleteMyPost(postIdToDelete, user.Id);
                        break;

                    case "5":
                        Console.WriteLine("Enter postid to comment on:");
                        int postIdToComment = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter your comment:");
                        string commentText = Console.ReadLine();
                        AddCommentToPost(postIdToComment, user.Id, commentText);
                        break;

                    case "6":
                        Console.WriteLine("Enter the search postName:");
                        string postName = Console.ReadLine();
                        SearchPosts(postName);
                        break;

                    case "7":
                        Console.WriteLine("Enter the group name:");
                        string groupName = Console.ReadLine();
                        CreateGroup(groupName, user.Id);
                        break;

                    case "8":
                        Console.WriteLine("Enter the group ID to join:");
                        int groupIdToJoin = int.Parse(Console.ReadLine());
                        JoinGroup(groupIdToJoin, user.Id);
                        break;

                    case "9":
                        Console.WriteLine("Enter the group ID to view posts:");
                        int groupIdToViewPosts = int.Parse(Console.ReadLine());
                        ViewGroupPosts(groupIdToViewPosts);
                        break;

                    case "10":
                        Console.WriteLine("Enter the group ID to add a new post:");
                        int groupIdToAddPost = int.Parse(Console.ReadLine());
                        AddPostToGroup(groupIdToAddPost, user.Id);
                        break;


                    case "11":
                        Console.WriteLine("Enter the post ID to comment on:");
                        int postIdToCommentGroup = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the group ID:");
                        int groupIdToComment = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter your comment:");
                        string groupPostCommentText = Console.ReadLine();
                        AddCommentToGroupPost(postIdToCommentGroup, groupIdToComment, user.Id, groupPostCommentText);
                        break;

                    case "12":
                        Console.WriteLine("Enter the group ID to display comments:");
                        int groupIdToDisplayComments = int.Parse(Console.ReadLine());
                        DisplayGroupComments(groupIdToDisplayComments);
                        break;

                    case "13":
                        Console.WriteLine("Enter the post ID to view comments:");
                        int postIdToViewComments = int.Parse(Console.ReadLine());
                        ViewPostComments(postIdToViewComments);
                        break;
                    case "14":
                        ViewAllGroups();
                        break;
                    case "15":
                  
                        Console.WriteLine("Enter the post ID to add to a group:");
                        int postIdToGroup = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the group ID:");
                        int groupIdToSharePost = int.Parse(Console.ReadLine());
                        SharePostToGroup(postIdToGroup, groupIdToSharePost);
                      

                        break;
                    case "16":
                        Console.WriteLine("Enter the ID of the post you want to edit:");
                        var postIdToEdit = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter new post title:");
                        var newTitle = Console.ReadLine();
                        Console.WriteLine("Enter new post content:");
                        var newContent = Console.ReadLine();
                        EditPost(postIdToEdit, user.Id, newTitle, newContent);
                        break;



                        break;
                    case "17":
                        Console.WriteLine("bye bye");
                        return;

                    default:
                        Console.WriteLine("error");
                        break;
                }
            }
        }



        private void AdminMenu(User admin)
        {
            AdminRepository adminRepository = new AdminRepository(_context);
            while (true)
            {
                Console.WriteLine("choose an action:");
                Console.WriteLine("1. view all Posts");
                Console.WriteLine("2. remove Post");
                Console.WriteLine("3. remove user");
                Console.WriteLine("4. view all comments");
                Console.WriteLine("5. remove group");
                Console.WriteLine("6. give user admin role");
                Console.WriteLine("7. view all users");
                Console.WriteLine("8. view user posts");
                Console.WriteLine("8. logout");
               
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllPost();
                        break;

                    case "2":
                        Console.Write("Enter PostId to remove: ");
                       var postId = int.Parse(Console.ReadLine());
                        adminRepository.RemovePost(postId);
                        break;
                    case "3":

                        Console.WriteLine("eneter user id to remove");
                        var userId = int.Parse(Console.ReadLine());
                        adminRepository.RemoveUser(userId);
                        break;
                        case "4":
                        adminRepository.ViewAllComments();
                        break;
                        case "5":
                        Console.WriteLine("enter group id to remove");
                         var groupId = int.Parse(Console.ReadLine());
                        adminRepository.RemoveGroup(groupId);
                        break;
                        case "6":
                        Console.WriteLine("enter user id to grand admin role");
                        userId = int.Parse(Console.ReadLine());
                        adminRepository.GiveUserAdminRole(userId);
                        break;
                    case "7":
                        adminRepository.ViewAllUsers();
                        break;
                    case "8":
                        Console.WriteLine("Enter user ID:");
                        int userId3 = int.Parse(Console.ReadLine());
                        adminRepository.ViewUserPosts(userId3);

                        break;
                    case "9":
                        Console.WriteLine("Bye bye");
                        return;

                    default:
                        Console.WriteLine("Try again");
                        break;
                }
            }
        }

        }
         }
    
