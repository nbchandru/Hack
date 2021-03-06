﻿using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace magHack.core.DataBaseAccess
{
    public class ODBConnection : IConnection
    {
        private OdbcConnection m_odbcConnection;

        private string m_connectionString;
            
        public ODBConnection(string connectionString)
        {
            m_connectionString = connectionString;
            m_connectionString = "";
            m_odbcConnection = new OdbcConnection(connectionString);
            TesteOleDB();
            m_odbcConnection.Open();
        }

        private void TesteOleDB()
        {
            {
                var tester = new OleDbConnection(Constants.OLEDBString);
                tester.Open();
                tester.Close();
            }
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
                    TesteOleDB();
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
                Close();
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public bool Reconnect()
        {
            Dispose(true);
            m_odbcConnection = new OdbcConnection(m_connectionString);
            return Connect();
        }

        public ICloneable GetDBConnection()
        {
            return m_odbcConnection;
        }
    }
}
