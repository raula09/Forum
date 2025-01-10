using Forum.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Forum.Models;
using System.Text.RegularExpressions;
   using System.Text.RegularExpressions;
namespace Forum.Models
{
    public class UserManagement
    {
        private ForumDbContext _context;

        public UserManagement(ForumDbContext context)
        {
            _context = context;
        }


        public void Register(string name, string password, string role)
        {
            try
            {
                
                if (name.Length < 5)
                {
                    Console.WriteLine("username needs to be 5 charaxchters long");
                    return;
                }

                
                if (_context.Users.Any(u => u.Username == name))
                {
                    Console.WriteLine("esername is already taken");
                    return;
                }

                
                if (!Regex.IsMatch(password, @"^(?=.*\d).{8,}$"))
                {
                    Console.WriteLine("password must include 8 charachters and a number");
                    return;
                }

                
                string hashedPassword = PasswordHelper.HashPassword(password);

               
                var user = new User
                {
                    Username = name,
                    PasswordHash = hashedPassword,
                    Role = role,
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                Console.WriteLine("Registered successfully.");
            }
            catch (Exception ex)
            {
         
            }
        }


            public User LoginUser(string name, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == name);
            if (user != null && PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
             
                return user;
            }
            else
            {
                
                return null;
            }
        }
    }
}
