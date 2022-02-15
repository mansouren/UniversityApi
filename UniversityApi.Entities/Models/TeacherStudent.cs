using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Common;

namespace UniversityApi.Entities.Models
{
   public class TeacherStudent : BaseEntity
    {
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }

        #region Relations
        
        public Teacher Teacher { get; set; }

        public Student Student { get; set; }
        #endregion
    }
}
