using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingAppAuthenticationAPI.Model
{
    public class BloggerDbContext: DbContext
    {
        public BloggerDbContext(DbContextOptions<BloggerDbContext> options)
            :base(options)
        { }

        public DbSet<Blogger> Bloggers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blogger>(entity =>
            {
                entity.HasKey("BloggerId");
                entity.Property("BloggerFullName")
                      .HasMaxLength(50);
                entity.Property("BloggerEmail");
                entity.Property("BloggerDOB");
                entity.Property("BloggerPasswordHash");
                entity.Property("BloggerSalt");
            });
        }
    }
}
