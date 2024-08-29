using ManagmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Core.BaseEntities
{
    public class BaseEntity
    {
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Status Status { get; set; }
    }
}
