
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School_English.Class;

namespace School_English.Interface
{
    internal interface IFilter
    {

        IEnumerable<ExtendedClient> GetFilterAndSort();
        int Count();
    }
}
