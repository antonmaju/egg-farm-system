using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace EggFarmSystem.Models
{
    public interface IValidatable
    {
        IList<ErrorInfo> Validate();
    }

    public abstract class Entity : IValidatable
    {
        public Guid Id { get; set; } 

        [Ignore]
        public bool IsNew { get { return Id == Guid.Empty; } }

        public virtual IList<ErrorInfo> Validate()
        {
            return null;
        }
    }
}
