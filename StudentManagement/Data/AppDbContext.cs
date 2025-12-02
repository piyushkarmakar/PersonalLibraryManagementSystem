using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using System.Collections.Generic;

namespace StudentManagement.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<Student> Students { get; set; } 
        }
    
}
