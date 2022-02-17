using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Common;

namespace UniversityApi.Entities.Models
{
   public class User : BaseEntity
    {
        public User()
        {
            IsActive = true;
            SecurityStamp = Guid.NewGuid();
        }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Guid SecurityStamp { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

        #region Relations
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        #endregion
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username).IsRequired().HasMaxLength(250);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
            builder.Property(u => u.FirstName).HasMaxLength(250);
            builder.Property(u => u.LastName).HasMaxLength(250);
            builder.Property(u => u.Phone).HasMaxLength(11);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(250);
            builder.Property(u => u.Address).HasMaxLength(600);

        }
    }
}
