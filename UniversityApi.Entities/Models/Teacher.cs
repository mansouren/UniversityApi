using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Common;

namespace UniversityApi.Entities.Models
{
    public class Teacher : BaseEntity
    {

        public int UserId { get; set; }

        [MaxLength(10)]
        public string TeacherCode { get; set; }

        #region Relations

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ICollection<TeacherStudent> TeacherStudents { get; set; }
        #endregion
    }
}
