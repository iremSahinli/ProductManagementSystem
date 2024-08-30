using ManagmentSystem.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Utilities.Concretes
{
    public class DataResult<T> : Result, IDataResult<T> where T : class
    {
        public T? Data { get; }

        public DataResult(T data, bool isSucces) : base(isSucces)
        {
            Data = data;
        }
        public DataResult(T data, bool isSucces, string message) : base(isSucces, message)
        {
            Data = data;
        }
    }
}
