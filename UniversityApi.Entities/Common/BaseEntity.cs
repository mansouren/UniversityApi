using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityApi.Entities.Common
{
    public interface IEntity
    {

    }
   public abstract class BaseEntity<Tkey> : IEntity 
    {
        [Key]
        public Tkey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
