using Microsoft.EntityFrameworkCore;
using Forum.Models;

namespace Forum.Data
{
    public class ForumDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupComment> GroupComments { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=RAUL\SQLEXPRESS;Database=ForumDb3;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


         
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction); 

           
            modelBuilder.Entity<GroupComment>()
                .HasOne(gc => gc.User)
                .WithMany(u => u.GroupComments)
                .HasForeignKey(gc => gc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupComment>()
                .HasOne(gc => gc.Post)
                .WithMany(p => p.GroupComments)
                .HasForeignKey(gc => gc.PostId)
                .OnDelete(DeleteBehavior.NoAction); 

        
            modelBuilder.Entity<GroupComment>()
                .HasOne(gc => gc.Group)
                .WithMany(g => g.GroupComments)
                .HasForeignKey(gc => gc.GroupId)
                .OnDelete(DeleteBehavior.NoAction); 
        }

    }

}


