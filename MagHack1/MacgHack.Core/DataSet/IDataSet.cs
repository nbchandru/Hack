using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacgHack.Core.Model;

namespace MacgHack.Core.DataSet
{
    public interface IDataSet : IDisposable
    {
        List<string> GetRow();
        bool MoveNext();
    }
}
