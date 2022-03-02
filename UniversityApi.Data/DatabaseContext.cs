using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Common;
using UniversityApi.Entities.Models;

namespace UniversityApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(entitiesAssembly);
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id =1 ,
            //        Username = "WebAdmin",
            //        Password = "123",
            //        RoleId = 1,
            //        IsActive = true
            //    }
            //);
        }
    }
}
