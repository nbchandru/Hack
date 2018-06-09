using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace MacgHack.Core.DataBaseAccess
{
    public interface IConnection : IDisposable
    {
        bool Connect();
        bool Reconnect();
        bool Close();

        OdbcDataReader CreateReaderWithQueryString(string queryString);
    }
}   
