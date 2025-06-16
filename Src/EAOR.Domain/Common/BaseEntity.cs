using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAOR.Domain.Common
{
    public abstract class BaseEntity : Entity
    {

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }

        protected BaseEntity() : base()
        {

        }

        protected BaseEntity(Guid id) : base(id)
        {
            
        }
    }
}
