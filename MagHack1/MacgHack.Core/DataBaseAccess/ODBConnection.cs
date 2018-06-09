using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacgHack.Core.DataBaseAccess
{
    public class ODBConnection : IConnection
    {
        private OdbcConnection m_odbcConnection;
        private OdbcCommand m_command;

        private string m_connectionString;

        public ODBConnection(string connectionString)
        {
            m_connectionString = connectionString;
            m_odbcConnection.Open();
            m_odbcConnection = new OdbcConnection(connectionString);
        }


        public bool Close()
        {
            try
            {
                m_odbcConnection.Close();
                return true;
            }
            catch (Exception)
            {
                //log
                return false;
            }
        }

        public bool Connect()
        {
            try
            {
                if (m_odbcConnection.State == System.Data.ConnectionState.Closed)
                {
                    m_odbcConnection.Open();
                }
                else
                {
                    //log
                }                
                return true;
            }
            catch (Exception)
            {
                // log
                return false;
            }
        }
        protected virtual void Dispose(bool clearManaged)
        {
            if (clearManaged)
            {
                Dispose();
            }
        }
        public void Dispose()
        {
            Close();
        }

        public bool Reconnect()
        {
            Dispose(true);
            m_odbcConnection = new OdbcConnection(m_connectionString);
            return Connect();
        }

        public OdbcDataReader CreateReaderWithQueryString(string queryString)
        {
            m_command = new OdbcCommand(queryString, m_odbcConnection);
            return m_command.ExecuteReader();
        }
    }
}
