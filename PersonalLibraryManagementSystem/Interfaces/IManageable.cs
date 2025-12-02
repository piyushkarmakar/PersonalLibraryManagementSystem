using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Interfaces
{
    public interface IManageable<T>
    {
        void Add(T item);
        void Update(int id, T item);
        void Remove(int id);
        T GetById(int id);

    }
}
