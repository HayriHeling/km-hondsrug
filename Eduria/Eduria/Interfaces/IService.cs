using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Interfaces
{
    interface IService<T>
    {
        EduriaContext Context { get; set; }
        IEnumerable<T> GetAll();
        void Add(T item);
        void Delete(T item);
        T GetById(int id);
    }
}
