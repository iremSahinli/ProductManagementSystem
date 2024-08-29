using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Core.Interfaces
{
    public interface ICreatableEntity : IEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
