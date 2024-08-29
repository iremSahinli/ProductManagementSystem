using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Core.Interfaces
{
    public interface IDeletableEntity
    {
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
