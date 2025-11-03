using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Interfaces
{
    public interface ISearchable<T>
    {
        List<T> Search(string criteria);
    }
}
