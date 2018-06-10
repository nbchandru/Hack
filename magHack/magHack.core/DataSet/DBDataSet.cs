using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magHack.core.DataBaseAccess;
using magHack.core.Model;

namespace magHack.core.DataSet
{
    public class DBDataSet : IDataSet
    {
        private OdbcDataReader m_dataReader;

        private int m_count;
        public List<string> ColumnNames
        {
            get
            {
                var count = m_dataReader.FieldCount;
                List<string> columnNames = new List<string>();
                for (int i = 0;i < count; i++)
                {
                    columnNames.Add(m_dataReader.GetName(i));
                }
                return columnNames;

            }
        }

        public int Count
        {
            get
            {
                return m_count;
            }
        }

        public DBDataSet(IConnection dbConnection, string queryString)
        {
            var con = dbConnection.GetDBConnection() as OdbcConnection;
            var commad = new OdbcCommand(queryString, con);
            m_dataReader = commad.ExecuteReader();
        }


        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                m_dataReader.Close();
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        public string GetValue(string columnName)
        {
            return m_dataReader.GetValue(ColumnNames.IndexOf(columnName)).ToString();
        }

        public bool MoveNext()
        {
            m_count++;
            return m_dataReader.Read();
        }
    }
}
