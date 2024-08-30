using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Utilities.Interfaces
{
    public interface IResult
    {
        public bool IsSucces { get; }
        public string Message { get; }
    }
}
