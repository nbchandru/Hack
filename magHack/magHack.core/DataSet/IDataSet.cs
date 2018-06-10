using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.DataSet
{
    public interface IDataSet : IDisposable
    {
        int Count { get; }
        List<string> ColumnNames { get; }
        string GetValue(string column);
        bool MoveNext();
    }
}
