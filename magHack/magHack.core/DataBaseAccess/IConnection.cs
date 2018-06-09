using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magHack.core.DataBaseAccess
{
    public interface IConnection : IDisposable
    {
        bool Connect();
        bool Reconnect();
        bool Close();

        OdbcDataReader CreateReaderWithQueryString(string queryString);
    }
}
