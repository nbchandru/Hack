using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using magHack.core.DataBaseAccess;

namespace magHack.core.Persister
{
    public class ODBCPersister : IDisposable
    {
        private OdbcConnection m_odbcConnection;
        private static Dictionary<string, Object> m_trxLock = new Dictionary<string, object>();
        private static object m_lock = new object();
        private static bool m_dicInit = false;
        const string clearAll = "clearAll";
        private void PrepareLockDictionary()
        {
            lock (m_lock)
            {
                if (!m_dicInit)
                {
                    FileStream fs = new FileStream(@"C:\hackathon\TableInfo.xml", FileMode.Open, FileAccess.Read);
                    var xmldoc = new XmlDocument();
                    xmldoc.Load(fs);
                    XmlNodeList xmlnodes;
                    xmlnodes = xmldoc.SelectNodes("//table");
                    foreach (XmlNode xmlnode in xmlnodes)
                    {
                        m_trxLock[xmlnode["name"].InnerText] = new object();
                    }
                    m_trxLock[clearAll] = new Object();
                    m_dicInit = true;
                    fs.Close();
                }
            }
        }
        public ODBCPersister(IConnection dbConnection)
        {
            m_odbcConnection = dbConnection.GetDBConnection() as OdbcConnection;
            PrepareLockDictionary();
        }
        public int ExecuteNonQueryCmd(string tableName, string cmd)
        {
            lock (m_trxLock[tableName])
            {
                var trx = m_odbcConnection.BeginTransaction();
                var odbcCmd = new OdbcCommand(cmd, m_odbcConnection, trx);
                var count = odbcCmd.ExecuteNonQuery();
                // log count
                trx.Commit();
                return count;
            }
        }

        public object ExecuteScalar(string cmd)
        {
            var odbcCmd = new OdbcCommand(cmd, m_odbcConnection);
            var obj = odbcCmd.ExecuteScalar();
            return obj;
        }
        public int Drop(string tableName)
        {
            lock (m_trxLock[clearAll])
            {
                return ExecuteNonQueryCmd(tableName, string.Format("drop table {0}",tableName));
            }
        }
        public int DropAllTables()
        {
            lock (m_trxLock[clearAll])
            {
                var trx = m_odbcConnection.BeginTransaction();
                var tableNames = m_trxLock.Where(x => x.Key != clearAll).Select(x => x.Key).ToList();
                var count = 0;
                foreach (var entry in tableNames)
                {
                    count += new OdbcCommand(string.Format("drop table {0}", entry), m_odbcConnection).ExecuteNonQuery();
                    // log count
                }

                trx.Commit();
                return count;
            }
        }
        public int ClearData(string tableName)
        {
            lock (m_trxLock[clearAll])
            {
                lock (m_trxLock[tableName])
                {
                    return ExecuteNonQueryCmd(tableName, string.Format("truncate {0}", tableName));
                }
            }
        }
        protected virtual void Dispose(bool diposeManaged)
        {
            if (diposeManaged)
            {
                // do dispose
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
