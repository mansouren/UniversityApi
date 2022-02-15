using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Common;

namespace UniversityApi.Entities.Models
{
   public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        #region Relations
        public IEnumerable<User> Users { get; set; }
        #endregion
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Name).IsRequired().HasMaxLength(20);
            builder.Property(r => r.Title).IsRequired().HasMaxLength(20);
            
        }
    }
}
