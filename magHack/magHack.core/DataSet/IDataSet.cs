using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.DataSet
{
    public interface IDataSet : IDisposable
    {
        List<string> GetRow();
        bool MoveNext();
    }
}
