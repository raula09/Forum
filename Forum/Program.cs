using Forum.Data;
using Forum.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class ForumApp
{
    public static void Main(string[] args)
    {
        var _context = new ForumDbContext();
        var userManagement = new UserManagement(_context);
        var forumRepository = new ForumRepository(_context);
        var adminRepository = new AdminRepository(_context);
        
        
        forumRepository.RegistrationMenu(userManagement);
    }
}
